using ProjectWPF.Validation;
using Repository;
using Repository.dto;
using Repository.Models.user;
using Service.product;
using Service.seller_request;
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
    /// Interaction logic for EditProductRequest.xaml
    /// </summary>
    public partial class EditProductRequest : Window
    {   
        private readonly long _productId;
        private readonly SellerRequestService _sellerRequestService;
        private readonly ProductUnitService _productUnitService;
        private readonly ProductService _productService;
        private readonly Product? _product;
        private readonly Seller _seller;
        public ProductUnit? selectedUnit;
        public EditProductRequest(long productId,
            SellerRequestService sellerRequestService, 
            ProductUnitService productUnitService, 
            ProductService productService,
            Seller seller)
        {
            _productId = productId;
            _sellerRequestService = sellerRequestService;
            _productUnitService = productUnitService;
            _productService = productService;
            _product =_productService.GetProductById(_productId)!;
            _seller = seller;
            if (_product == null)
            {
                MessageBox.Show("Không tìm thấy sản phẩm", "Thông báo",
                               MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
                return;
            }
            InitializeComponent();
            InitData();
        }

        private void InitData()
        {
            ProductNameTextBox.Text = _product?.Name;
            DescriptionTextBox.Text = _product?.Description;
            PriceTextBox.Text = _product?.Price.ToString();
            IsActiveCheckBox.IsChecked = _product?.IsActive;
            var productUnits = _productUnitService.GetAll();
            UnitComboBox.ItemsSource = productUnits;
            UnitComboBox.DisplayMemberPath = "Name";
            UnitComboBox.SelectedValuePath = "Id";
            UnitComboBox.SelectedValue = _product?.ProductUnitId;
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            ProductDto product = new ProductDto
            {
                Id = _productId,
                Name = ProductNameTextBox.Text,
                Description = DescriptionTextBox.Text,
                Price = PriceTextBox.Text,
                IsActive = IsActiveCheckBox.IsChecked ?? false,
                ProductUnit = _productUnitService.GetById((long)UnitComboBox.SelectedValue)!
            };
            ProductValidator validations = new ProductValidator(_productService);
            var result = validations.Validate(product);
            if (!result.IsValid)
            {
                MessageBox.Show(result.Errors.First().ErrorMessage, "Lỗi",
                               MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
               Product p = new Product
                {
                    Id = product.Id,
                    Name = product.Name!,
                    Description = product.Description!,
                    Price = int.Parse(product.Price!),
                    IsActive = product.IsActive,
                    ProductUnitId = product.ProductUnit.Id,
                    ProductUnit = product.ProductUnit
                };
                _sellerRequestService.SaveUpdateRequest(p, _productService.GetProductById(_productId),_seller);
                MessageBox.Show("Gửi yêu cầu chỉnh sửa sản phẩm thành công!\nYêu cầu của bạn sẽ được xem xét bởi quản trị viên.", 
                               "Thành Công", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc chắn muốn hủy? Các thay đổi sẽ không được lưu.", 
                                        "Xác Nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }
    }
}
