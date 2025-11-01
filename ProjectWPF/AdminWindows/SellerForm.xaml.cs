using Repository;
using Repository.Models.user;
using Service;
using Service.user;
using System.Windows;
using System.Windows.Controls;

namespace ProjectWPF.AdminWindows
{
    /// <summary>
    /// Interaction logic for AddSeller.xaml
    /// </summary>
    public partial class SellerForm : Window
    {
        private readonly SellerService _sellerService;
        private readonly AddressService _addressService;

        private Seller? _sellerToUpdate;
        private bool _isInUpdateMode;

        public SellerForm(SellerService sellerService,
            AddressService addressService)
        {
            _sellerService = sellerService;
            _addressService = addressService;

            InitializeComponent();
        }

        public void SetSellerToUpdate(Seller seller)
        {
            _sellerToUpdate = seller;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var provinceCities = _addressService.GetAllProvinceCities();
            provinceCities.Insert(0, new() { Code = "default", Name = "Chọn Tỉnh/Thành Phố" });
            ProvinceCityComboBox.ItemsSource = provinceCities;

            if (_sellerToUpdate == null)
            {
                Title = "Thêm Mới Người Bán";
                SaveSellerTitleTextBlock.Text = "Thêm Mới Người Bán";
                SaveSellerButton.Content = "Thêm Mới Người Bán";
                PasswordHintLabel.Visibility = Visibility.Collapsed;
                UpdateSellerAskButton.Visibility = Visibility.Collapsed;
                DeleteSellerButton.Visibility = Visibility.Collapsed;
                CommuneWardComboBox.ItemsSource = new List<CommuneWard> { new() { Code = "default", Name = "Chọn Xã/Phường", ProvinceCityCode = "0" } };
                ProvinceCityComboBox.SelectedIndex = 0;
                CommuneWardComboBox.SelectedIndex = 0;
                StatusCheckBox.IsChecked = true;
            }
            else
            {
                Title = "Thông Tin Người Bán";
                SaveSellerTitleTextBlock.Text = "Thông Tin Người Bán";
                SaveSellerButton.Content = "Lưu Thay Đổi";
                InitShowDetailMode();
            }
        }

        private void UpdateSellerAskButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_isInUpdateMode)
            {
                InitUpdateMode();
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn muốn huỷ cập nhật người bán?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result != MessageBoxResult.Yes) return;
                InitShowDetailMode();
            }
        }

        private void ProvinceCityComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedValue = (string)ProvinceCityComboBox.SelectedValue;
            if (selectedValue == "default")
            {
                CommuneWardComboBox.ItemsSource = new List<CommuneWard> { new() { Code = "default", Name = "Chọn Xã/Phường", ProvinceCityCode = "0" } };
                CommuneWardComboBox.SelectedIndex = 0;
                return;
            }
            var communeWards = _addressService.GetCommuneWardsByProvinceCityCode(selectedValue);
            communeWards.Insert(0, new() { Code = "default", Name = "Chọn Xã/Phường", ProvinceCityCode = "0" });
            CommuneWardComboBox.ItemsSource = communeWards;
            CommuneWardComboBox.SelectedIndex = 0;
        }

        private void SaveSellerButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text;
            string password = PasswordTextBox.Text;
            string fullName = FullNameTextBox.Text;
            DateOnly? birthDate = BirthDatePicker.SelectedDate.HasValue ? DateOnly.FromDateTime(BirthDatePicker.SelectedDate.Value) : null;
            string identify = IdentifyTextBox.Text;
            string provinceCity = (string)ProvinceCityComboBox.SelectedValue;
            string communneWard = (string)CommuneWardComboBox.SelectedValue;
            string specificAddress = SpecificAddressTextBox.Text;
            bool isActive = StatusCheckBox.IsChecked == true;
            try
            {
                if (_sellerToUpdate == null)
                {
                    MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn muốn thêm mới người bán?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result != MessageBoxResult.Yes) return;

                    _sellerService.AddNewSeller(email, password, fullName, birthDate, identify, provinceCity, communneWard, specificAddress, isActive);

                    MessageBox.Show("Thêm mới người bán thành công.", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                else
                {
                    MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn muốn cập nhật người bán?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result != MessageBoxResult.Yes) return;

                    _sellerService.UpdateExistingSeller(_sellerToUpdate, password, fullName, birthDate, identify, provinceCity, communneWard, specificAddress, isActive);

                    InitShowDetailMode();
                    MessageBox.Show("Cập nhật người bán thành công.", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn muốn đặt lại tất cả thay đổi?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) return;

            if (_sellerToUpdate == null)
            {
                EmailTextBox.Text = "";
                EmailTextBox.IsHitTestVisible = true;
                PasswordTextBox.Text = "";
                FullNameTextBox.Text = "";
                BirthDatePicker.SelectedDate = null;
                IdentifyTextBox.Text = "";
                ProvinceCityComboBox.SelectedIndex = 0;
                CommuneWardComboBox.ItemsSource = new List<CommuneWard> { new() { Code = "default", Name = "Chọn Xã/Phường", ProvinceCityCode = "0" } };
                CommuneWardComboBox.SelectedIndex = 0;
                SpecificAddressTextBox.Text = "";
                StatusCheckBox.IsChecked = true;
            }
            else
            {
                AutoFillSellerInfo();
            }
        }

        private void DeleteSellerButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn muốn xoá người bán?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) return;

            _sellerService.DeleteSeller(_sellerToUpdate!.Id);
            MessageBox.Show("Xoá người bán thành công.", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private void InitShowDetailMode()
        {
            _isInUpdateMode = false;

            EmailTextBox.IsHitTestVisible = false;
            PasswordTextBox.Visibility = Visibility.Collapsed;
            PasswordLabel.Visibility = Visibility.Collapsed;
            PasswordHintLabel.Visibility = Visibility.Collapsed;
            FullNameTextBox.IsHitTestVisible = false;
            BirthDatePicker.IsHitTestVisible = false;
            IdentifyTextBox.IsHitTestVisible = false;
            ProvinceCityComboBox.IsHitTestVisible = false;
            CommuneWardComboBox.IsHitTestVisible = false;
            SpecificAddressTextBox.IsHitTestVisible = false;
            StatusCheckBox.IsHitTestVisible = false;
            UpdateSellerAskButton.Content = "Cập Nhật Người Bán";
            SaveSellerButton.Visibility = Visibility.Collapsed;
            ResetButton.Visibility = Visibility.Collapsed;
            DeleteSellerButton.Visibility = Visibility.Collapsed;
            AutoFillSellerInfo();
        }

        private void InitUpdateMode()
        {
            _isInUpdateMode = true;

            PasswordTextBox.Visibility = Visibility.Visible;
            PasswordLabel.Visibility = Visibility.Visible;
            PasswordHintLabel.Visibility = Visibility.Visible;
            FullNameTextBox.IsHitTestVisible = true;
            BirthDatePicker.IsHitTestVisible = true;
            IdentifyTextBox.IsHitTestVisible = true;
            ProvinceCityComboBox.IsHitTestVisible = true;
            CommuneWardComboBox.IsHitTestVisible = true;
            SpecificAddressTextBox.IsHitTestVisible = true;
            StatusCheckBox.IsHitTestVisible = true;
            UpdateSellerAskButton.Content = "Huỷ Cập Nhật";
            SaveSellerButton.Visibility = Visibility.Visible;
            ResetButton.Visibility = Visibility.Visible;
            DeleteSellerButton.Visibility = Visibility.Visible;
        }

        private void AutoFillSellerInfo()
        {
            string? sellerProvinceCityCode = _sellerToUpdate!.CommuneWard?.ProvinceCityCode;
            ProvinceCityComboBox.SelectedValue = sellerProvinceCityCode;
            var communeWards = _addressService.GetCommuneWardsByProvinceCityCode(sellerProvinceCityCode!);
            communeWards.Insert(0, new() { Code = "default", Name = "Chọn Xã/Phường", ProvinceCityCode = "0" });
            CommuneWardComboBox.ItemsSource = communeWards;
            CommuneWardComboBox.SelectedValue = _sellerToUpdate.CommuneWardCode;
            EmailTextBox.Text = _sellerToUpdate.Email;
            PasswordTextBox.Text = "";
            FullNameTextBox.Text = _sellerToUpdate.FullName;
            BirthDatePicker.SelectedDate = _sellerToUpdate.BirthDate.ToDateTime(TimeOnly.MinValue);
            IdentifyTextBox.Text = _sellerToUpdate.Cid;
            SpecificAddressTextBox.Text = _sellerToUpdate.SpecificAddress;
            StatusCheckBox.IsChecked = _sellerToUpdate.IsActive;
        }

    }
}
