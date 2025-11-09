using ProjectWPF.Validation;
using Repository;
using Repository.dto;
using Repository.Models.user;
using Service.product;
using Service.seller_request;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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
        private ObservableCollection<ProductBatchDto> _productBatches;

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
            _product = _productService.GetProductById(_productId)!;
            _seller = seller;
            _productBatches = new ObservableCollection<ProductBatchDto>();

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

            // Load existing product batches
            if (_product?.ProductBatches != null && _product.ProductBatches.Any())
            {
                foreach (var batch in _product.ProductBatches)
                {
                    _productBatches.Add(new ProductBatchDto
                    {
                        Id = batch.Id,
                        ExpiryDate = batch.ExpiryDate.ToString("dd/MM/yyyy"),
                        Quantity = batch.Quantity.ToString(),
                        ProductId = _productId
                    });
                }
            }

            // Initialize batches list
            BatchesItemsControl.ItemsSource = _productBatches;
            UpdateBatchesDisplay();
        }

        private void AddBatch_Click(object sender, RoutedEventArgs e)
        {
            NewBatchPanel.Visibility = Visibility.Visible;
            ExpiryDatePicker.SelectedDate = DateTime.Now.AddMonths(6);
            QuantityTextBox.Text = "";
            BatchIdTextBox.Tag = null; // Clear any edit state
            QuantityTextBox.Focus();
        }

        private void SaveBatch_Click(object sender, RoutedEventArgs e)
        {
            // Validate inputs
            if (!ExpiryDatePicker.SelectedDate.HasValue)
            {
                MessageBox.Show("Vui lòng chọn hạn sử dụng", "Lỗi", 
                               MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (ExpiryDatePicker.SelectedDate.Value.Date <= DateTime.Now.Date)
            {
                MessageBox.Show("Hạn sử dụng phải là ngày trong tương lai", "Lỗi", 
                               MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(QuantityTextBox.Text))
            {
                MessageBox.Show("Vui lòng nhập số lượng", "Lỗi", 
                               MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(QuantityTextBox.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Số lượng phải là số nguyên dương", "Lỗi", 
                               MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Check if editing existing batch
            if (BatchIdTextBox.Tag is ProductBatchDto existingBatch)
            {
                // Update existing batch
                existingBatch.ExpiryDate = ExpiryDatePicker.SelectedDate.Value.ToString("dd/MM/yyyy");
                existingBatch.Quantity = quantity.ToString();
                
                // Refresh the display
                BatchesItemsControl.Items.Refresh();
                
                MessageBox.Show("Đã cập nhật lô hàng thành công!", "Thành Công", 
                               MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                // Add new batch
                var newBatch = new ProductBatchDto
                {
                    ExpiryDate = ExpiryDatePicker.SelectedDate.Value.ToString("dd/MM/yyyy"),
                    Quantity = quantity.ToString(),
                    ProductId = _productId
                };

                _productBatches.Add(newBatch);
                
                MessageBox.Show("Đã thêm lô hàng thành công!", "Thành Công", 
                               MessageBoxButton.OK, MessageBoxImage.Information);
            }

            UpdateBatchesDisplay();

            // Clear inputs and hide panel
            NewBatchPanel.Visibility = Visibility.Collapsed;
            ExpiryDatePicker.SelectedDate = null;
            QuantityTextBox.Text = "";
            BatchIdTextBox.Tag = null;
        }

        private void EditBatch_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is ProductBatchDto batch)
            {
                // Show the edit panel
                NewBatchPanel.Visibility = Visibility.Visible;
                
                // Populate fields with existing batch data
                ExpiryDatePicker.SelectedDate = DateTime.ParseExact(batch.ExpiryDate!, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                QuantityTextBox.Text = batch.Quantity;
                
                // Store reference to the batch being edited
                BatchIdTextBox.Tag = batch;
                
                QuantityTextBox.Focus();
            }
        }

        private void RemoveBatch_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is ProductBatchDto batch)
            {
                var result = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa lô hàng này?\nHạn sử dụng: {batch.ExpiryDate}\nSố lượng: {batch.Quantity}", 
                    "Xác Nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _productBatches.Remove(batch);
                    UpdateBatchesDisplay();
                    MessageBox.Show("Đã xóa lô hàng!", "Thành Công", 
                                   MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void UpdateBatchesDisplay()
        {
            NoBatchesText.Visibility = _productBatches.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
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

            // Create ProductWithBatchesDto
            ProductWithBatchesDto productWithBatches = new ProductWithBatchesDto
            {
                Id = product.Id,
                Name = product.Name!,
                Description = product.Description!,
                Price = product.Price!,
                IsActive = product.IsActive,
                ProductUnit = product.ProductUnit,
                ProductBatches = _productBatches.ToList()
            };

            Product p = new Product
            {
                Id = product.Id,
                Name = product.Name!,
                Description = product.Description!,
                Price = int.Parse(product.Price!),
                IsActive = product.IsActive,
                ProductUnitId = product.ProductUnit.Id,
                ProductUnit = product.ProductUnit,
                ProductBatches = _productBatches.Select(b => new ProductBatch
                {
                    Id = b.Id ?? 0, // Preserve existing batch IDs
                    ExpiryDate = DateTime.ParseExact(b.ExpiryDate!, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Quantity = int.Parse(b.Quantity!),
                    ProductId = _productId
                }).ToList()
            };

            _sellerRequestService.SaveUpdateRequest(p, _productService.GetProductById(_productId), _seller);
            
            string batchInfo = _productBatches.Count > 0 
                ? $"\n\nĐã gửi yêu cầu với {_productBatches.Count} lô hàng." 
                : "";

            MessageBox.Show($"Gửi yêu cầu chỉnh sửa sản phẩm thành công!{batchInfo}\n\nYêu cầu của bạn sẽ được xem xét bởi quản trị viên.", 
                           "Thành Công", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
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

        private void CancelBatchEdit_Click(object sender, RoutedEventArgs e)
        {
            NewBatchPanel.Visibility = Visibility.Collapsed;
            ExpiryDatePicker.SelectedDate = null;
            QuantityTextBox.Text = "";
            BatchIdTextBox.Tag = null;
        }
    }
}
