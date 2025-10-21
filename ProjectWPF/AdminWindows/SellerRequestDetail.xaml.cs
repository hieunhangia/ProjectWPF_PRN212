using ProjectWPF.Models;
using Repository;
using Service.product;
using Service.seller_request;
using System.Text.Json;
using System.Windows;

namespace ProjectWPF.AdminWindows
{
    /// <summary>
    /// Interaction logic for SellerRequestDetail.xaml
    /// </summary>
    public partial class SellerRequestDetail : Window
    {
        private readonly SellerRequestService _sellerRequestService;
        private readonly long _sellerRequestId;
        private readonly ProductService _productService;
        private readonly ProductUnitService _productUnitService;
        public Product? OldProduct { get; set; }
        public Product? NewProduct { get; set; }
        public SellerRequestDetail(long sellerRequestId
            , SellerRequestService sellerRequestService
            ,ProductService productService
            ,ProductUnitService productUnitService)
        {
            _productUnitService = productUnitService;
            _sellerRequestId = sellerRequestId;
            _sellerRequestService = sellerRequestService;
            _productService = productService;
            InitializeComponent();
            InitData();
        }

        private void InitData()
        {
            SellerRequest sellerRequest = _sellerRequestService.getSellerRequestById(_sellerRequestId)!;
            if (sellerRequest == null)
            {
                MessageBox.Show("Yêu cầu không tồn tại", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
                return;
            }
            NewProduct = JsonSerializer.Deserialize<Product>(sellerRequest.Content)!;

            if (NewProduct == null)
            {
                MessageBox.Show("Nội dung yêu cầu không hợp lệ", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
                return;
            }
            NewProduct.ProductUnit = _productUnitService.GetById(NewProduct.ProductUnitId)!;
            long id = sellerRequest.OldContentId ?? 0;
            OldProduct = _productService.GetProductById(id);
            DataContext = this;
            if (OldProduct == null)
            {
                OldProductBox.Visibility = Visibility.Collapsed;
            }
            else
            {
                OldProduct.ProductUnit = _productUnitService.GetById(OldProduct.ProductUnitId)!;
            }
        }

        private void AcceptRequest_Click(object sender, RoutedEventArgs e)
        {
            _sellerRequestService.approveRequest<Product>(_sellerRequestId, _productService.AddProduct, _productService.UpdateProduct);
            this.Close();
            MessageBox.Show("Thành công chấp nhận yêu cầu sản phẩm");
        }
        
        private void RejectRequest_Click(object sender, RoutedEventArgs e)
        {
            _sellerRequestService.RejectRequest(_sellerRequestId);
            this.Close();
            MessageBox.Show("Thành công huỷ yêu cầu thay đổi sản phẩm");
        }
    }
}
