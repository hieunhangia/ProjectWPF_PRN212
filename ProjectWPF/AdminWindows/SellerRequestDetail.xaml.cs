using Grpc.Core;
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
        public string StatusName { get; init; }
        public Product? OldProduct { get; init; }
        public Product? NewProduct { get; init; }
        public SellerRequestDetail(long sellerRequestId
            , SellerRequestService sellerRequestService
            ,ProductService productService
            ,ProductUnitService productUnitService)
        {
            _productUnitService = productUnitService;
            _sellerRequestId = sellerRequestId;
            _sellerRequestService = sellerRequestService;
            _productService = productService;
            SellerRequest sellerRequest = _sellerRequestService.getSellerRequestById(_sellerRequestId)!;
            StatusName = _sellerRequestService.getSellerRequestById(sellerRequestId)?.Status.Name!;
            NewProduct = JsonSerializer.Deserialize<Product>(sellerRequest.Content);
            if (sellerRequest.OldContent != null)
            {
                OldProduct = JsonSerializer.Deserialize<Product>(sellerRequest.OldContent);
            }
            InitializeComponent();
            InitData();
        }

        private void InitData()
        {

            if (NewProduct == null)
            {
                MessageBox.Show("Nội dung yêu cầu không hợp lệ", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
                return;
            }
            DataContext = this;
            if (OldProduct == null)
            {
                OldProductBox.Visibility = Visibility.Collapsed;
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

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
