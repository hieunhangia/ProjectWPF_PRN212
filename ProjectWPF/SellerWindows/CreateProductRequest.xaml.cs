using ProjectWPF.AdminWindows;
using ProjectWPF.dto;
using ProjectWPF.Validation;
using Repository;
using Repository.Models.user;
using Service.product;
using Service.seller_request;
using Service.user;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProjectWPF.SellerWindows
{
    /// <summary>
    /// Interaction logic for CreateProductRequest.xaml
    /// </summary>
    public partial class CreateProductRequest : Window
    {
        private readonly SellerRequestService _sellerRequestService;
        private readonly ProductService _productService;
        private readonly ProductUnitService _productUnitService;
        private readonly NavigationWindow _navigationWindow;
        private Seller? _loggedInSeller;

        public CreateProductRequest(SellerRequestService sellerRequestService, 
                                    ProductService productService,
                                    ProductUnitService productUnitService,
                                    NavigationWindow navigationWindow)
        {
            _navigationWindow = navigationWindow;
            _sellerRequestService = sellerRequestService;
            _productService = productService;
            _productUnitService = productUnitService;
            InitializeComponent();
            InitData();
        }

        public void SetLoggedInSeller(Seller seller)
        {
            _loggedInSeller = seller;
        }

        private void InitData()
        {
            UnitComboBox.ItemsSource = _productUnitService.GetAll();
            UnitComboBox.SelectedIndex = 0;
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if(_loggedInSeller == null)
            {
                MessageBox.Show("Lỗi trong hệ thống đăng nhập", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var productUnit = UnitComboBox.SelectedItem as ProductUnit;
            CreateProductDto p = new CreateProductDto()
            {
                Name = ProductNameTextBox.Text,
                Description = DescriptionTextBox.Text,
                Price = PriceTextBox.Text,
                IsActive = IsActiveCheckBox.IsChecked ?? false,
                ProductUnit = productUnit!
            };
            
            var validator = new CreateProductValidator(_productService);
            var result = validator.Validate(p);
            if (!result.IsValid)
            {
                MessageBox.Show(result.Errors.First().ErrorMessage, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                Product product = new Product()
                {
                    Name = p.Name,
                    Description = p.Description,
                    Price = int.Parse(p.Price),
                    IsActive = p.IsActive,
                    ProductUnit = p.ProductUnit!
                };
                _sellerRequestService.SaveAddRequest(product, _loggedInSeller);
                _navigationWindow.ShowWindowAndCloseCurrent<MainWindow>(this);
                MessageBox.Show("Thêm yêu cầu tạo sản phẩm thành công");
            }
        }
    }
}
