using Microsoft.Extensions.DependencyInjection;
using ProjectWPF.AdminWindows;
using Repository;
using Repository.Models.user;
using Service.product;
using System.Windows;
using System.Windows.Controls;

namespace ProjectWPF.SellerWindows
{
    /// <summary>
    /// Interaction logic for ProductList.xaml
    /// </summary>
    public partial class ProductList : Window
    {
        private readonly ProductService _productService;
        private readonly ProductUnitService _productUnitService;
        private readonly NavigationWindow _navigationWindow;
        private readonly IServiceProvider _serviceProvider;
        private Seller? _loggedInSeller;
        public ProductList(ProductService productService, 
                          ProductUnitService productUnitService,
                          NavigationWindow navigationWindow,
                          IServiceProvider serviceProvider)
        {
            _productService = productService;
            _productUnitService = productUnitService;
            _navigationWindow = navigationWindow;
            _serviceProvider = serviceProvider;
            InitializeComponent();
            LoadProducts();
        }

        private void LoadProducts()
        {
            try
            {
                var products = _productService.GetAllProducts();
                
                // Load ProductUnit information for each product
                foreach (var product in products)
                {
                    if (product.ProductUnit == null)
                    {
                        product.ProductUnit = _productUnitService.GetById(product.ProductUnitId);
                    }
                }
                
                ProductsDataGrid.ItemsSource = products;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách sản phẩm: {ex.Message}", 
                               "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ViewDetail_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button || button.Tag is not long productId)
                return;

            try
            {
                var product = _productService.GetProductById(productId);
                if (product != null)
                {
                    if (product.ProductUnit == null)
                    {
                        product.ProductUnit = _productUnitService.GetById(product.ProductUnitId);
                    }
                    
                    var detailWindow = new ProductDetail(product);
                    detailWindow.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy sản phẩm", "Thông báo", 
                                   MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xem chi tiết sản phẩm: {ex.Message}", 
                               "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button || button.Tag is not long productId)
                return;

            try
            {
                var sellerRequestDetailFactory = _serviceProvider.GetRequiredService<Func<long, EditProductRequest>>();
                var editWindow = sellerRequestDetailFactory(productId);
                editWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi chỉnh sửa sản phẩm: {ex.Message}", 
                               "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            var createProductWindow = _navigationWindow.GetWindow<CreateProductRequest>();
            createProductWindow.SetLoggedInSeller(_loggedInSeller!);
            createProductWindow.Show();
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadProducts();
            MessageBox.Show("Danh sách sản phẩm đã được làm mới", "Thông báo", 
                           MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        internal void SetLoggedInSeller(Seller seller)
        {
            _loggedInSeller = seller;
        }
    }
}