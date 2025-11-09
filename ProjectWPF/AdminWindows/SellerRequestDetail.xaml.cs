using ProjectWPF.Models;
using Repository;
using Repository.dto;
using Service.product;
using Service.seller_request;
using System.Collections.Generic;
using System.Linq;
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
        public List<ProductBatchDto> NewProductBatches { get; init; }
        public List<ProductBatchDto> OldProductBatches { get; init; }
        public Visibility NewProductBatchesVisibility { get; init; }
        public Visibility OldProductBatchesVisibility { get; init; }

        public SellerRequestDetail(long sellerRequestId,
            SellerRequestService sellerRequestService,
            ProductService productService,
            ProductUnitService productUnitService)
        {
            _productUnitService = productUnitService;
            _sellerRequestId = sellerRequestId;
            _sellerRequestService = sellerRequestService;
            _productService = productService;
            
            SellerRequest sellerRequest = _sellerRequestService.getSellerRequestById(_sellerRequestId)!;
            StatusName = _sellerRequestService.getSellerRequestById(sellerRequestId)?.Status.Name!;
            
            NewProduct = JsonSerializer.Deserialize<Product>(sellerRequest.Content);
            NewProductBatches = new List<ProductBatchDto>();
            OldProductBatches = new List<ProductBatchDto>();
            
            if (NewProduct?.ProductBatches != null && NewProduct.ProductBatches.Any())
            {
                NewProductBatches = NewProduct.ProductBatches.Select(b => new ProductBatchDto
                {
                    Id = b.Id,
                    ExpiryDate = b.ExpiryDate.ToString("dd/MM/yyyy"),
                    Quantity = b.Quantity.ToString(),
                    ProductId = b.ProductId
                }).ToList();
                NewProductBatchesVisibility = Visibility.Visible;
            }
            else
            {
                NewProductBatchesVisibility = Visibility.Collapsed;
            }
            
            if (sellerRequest.OldContent != null)
            {
                OldProduct = JsonSerializer.Deserialize<Product>(sellerRequest.OldContent);
                
                // Check if Old Product has batches
                if (OldProduct?.ProductBatches != null && OldProduct.ProductBatches.Any())
                {
                    OldProductBatches = OldProduct.ProductBatches.Select(b => new ProductBatchDto
                    {
                        Id = b.Id,
                        ExpiryDate = b.ExpiryDate.ToString("dd/MM/yyyy"),
                        Quantity = b.Quantity.ToString(),
                        ProductId = b.ProductId
                    }).ToList();
                    OldProductBatchesVisibility = Visibility.Visible;
                }
                else
                {
                    OldProductBatchesVisibility = Visibility.Collapsed;
                }
            }
            else
            {
                OldProductBatchesVisibility = Visibility.Collapsed;
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
            string batchInfo = "";
            if (NewProductBatches.Count > 0)
            {
                batchInfo = $"\n\n📦 Sản phẩm có {NewProductBatches.Count} lô hàng sẽ được cập nhật vào hệ thống.";
            }

            var confirmResult = MessageBox.Show(
                $"Bạn có chắc chắn muốn chấp thuận yêu cầu này?{batchInfo}", 
                "Xác Nhận", 
                MessageBoxButton.YesNo, 
                MessageBoxImage.Question);

            if (confirmResult == MessageBoxResult.Yes)
            {
                
                _sellerRequestService.approveRequest<Product>(_sellerRequestId, _productService.AddProduct, _productService.UpdateProduct);
                if(OldProduct != null)
                {
                    _sellerRequestService.UpdateOldHistory<Product>(_sellerRequestId, OldProduct);
                }
                this.Close();
                MessageBox.Show("Thành công chấp nhận yêu cầu sản phẩm" + batchInfo, "Thành Công", 
                               MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        
        private void RejectRequest_Click(object sender, RoutedEventArgs e)
        {
            var confirmResult = MessageBox.Show(
                "Bạn có chắc chắn muốn từ chối yêu cầu này?", 
                "Xác Nhận", 
                MessageBoxButton.YesNo, 
                MessageBoxImage.Question);

            if (confirmResult == MessageBoxResult.Yes)
            {
                _sellerRequestService.RejectRequest(_sellerRequestId);
                this.Close();
                MessageBox.Show("Thành công huỷ yêu cầu thay đổi sản phẩm", "Thành Công", 
                               MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
