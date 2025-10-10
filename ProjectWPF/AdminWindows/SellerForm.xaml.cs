using Repository;
using Repository.Models.user;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProjectWPF.AdminWindows
{
    /// <summary>
    /// Interaction logic for AddSeller.xaml
    /// </summary>
    public partial class SellerForm : Window
    {

        private readonly Seller? _sellerToUpdate;
        private readonly SellerService _sellerService = new();
        private readonly AddressService _addressService = new();
        private bool _isInUpdateMode;

        public SellerForm()
        {
            InitializeComponent();
        }

        public SellerForm(Seller? seller) : this()
        {
            _sellerToUpdate = seller;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var provinceCities = _addressService.GetAllProvinceCities();
            provinceCities.Insert(0, new() { Code = "default", Name = "Chọn Tỉnh/Thành Phố" });
            ProvinceCityComboBox.ItemsSource = provinceCities;

            DeleteSellerButton.Visibility = Visibility.Collapsed;

            if (_sellerToUpdate == null)
            {
                Title = "Thêm Mới Người Bán";
                SaveSellerTitleTextBlock.Text = "Thêm Mới Người Bán";
                SaveSellerButton.Content = "Thêm Mới Người Bán";
                UpdateSellerAskButton.Visibility = Visibility.Collapsed;
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
            communeWards.Insert(0, new () { Code = "default", Name = "Chọn Xã/Phường", ProvinceCityCode = "0" });
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

            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(fullName) ||
                birthDate == null ||
                string.IsNullOrWhiteSpace(identify) ||
                provinceCity == "default" ||
                communneWard == "default" ||
                string.IsNullOrWhiteSpace(specificAddress))
            {
                MessageBox.Show("Hãy cung cấp đầy đủ thông tin.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                static bool IsValidEmail(string email)
                {
                    try
                    {
                        var addr = new MailAddress(email);
                        return addr.Address == email;
                    }
                    catch
                    {
                        return false;
                    }
                }
                if (!IsValidEmail(email))
                {
                    MessageBox.Show("Email không hợp lệ.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                static bool IsValidPassword(string password) =>
                    password.Length >= 6
                    && password.Length <= 50
                    && password.Any(char.IsLower)
                    && password.Any(char.IsUpper)
                    && password.Any(char.IsDigit)
                    && password.Any(ch => !char.IsLetterOrDigit(ch));
                if (!IsValidPassword(password))
                {
                    MessageBox.Show("Mật khẩu phải trong khoảng 6-50 ký tự, bao gồm chữ hoa, chữ thường, số và ký tự đặc biệt.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (birthDate > DateOnly.FromDateTime(DateTime.Now))
                {
                    MessageBox.Show("Ngày sinh không hợp lệ.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                static bool IsValidID(string identify) => (identify.Length == 9 || identify.Length == 12) && identify.All(char.IsDigit);
                if (!IsValidID(identify))
                {
                    MessageBox.Show("CMND/CCCD không hợp lệ.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (_sellerToUpdate == null)
                {
                    if (_sellerService.IsEmailExists(email))
                    {
                        MessageBox.Show("Email đã tồn tại.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    if (_sellerService.IsIdentifyExists(identify))
                    {
                        MessageBox.Show("CMND/CCCD đã tồn tại.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    Seller seller = new()
                    {
                        Email = email,
                        Password = password,
                        IsActive = isActive,
                        FullName = fullName,
                        BirthDate = birthDate.Value,
                        Cid = identify,
                        SpecificAddress = specificAddress,
                        CommuneWardCode = communneWard
                    };

                    _sellerService.AddSeller(seller);
                    MessageBox.Show("Thêm người bán thành công.", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                else
                {
                    if (_sellerService.IsIdentifyExists(identify, _sellerToUpdate.Id))
                    {
                        MessageBox.Show("CMND/CCCD đã tồn tại.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn muốn cập nhật người bán?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result != MessageBoxResult.Yes) return;

                    _sellerToUpdate.Password = password;
                    _sellerToUpdate.FullName = fullName;
                    _sellerToUpdate.BirthDate = birthDate.Value;
                    _sellerToUpdate.Cid = identify;
                    _sellerToUpdate.SpecificAddress = specificAddress;
                    _sellerToUpdate.CommuneWard = _addressService.GetCommuneWardByCode(communneWard);
                    _sellerToUpdate.IsActive = isActive;
                    _sellerService.UpdateSeller(_sellerToUpdate);
                    InitShowDetailMode();
                    MessageBox.Show("Cập nhật người bán thành công.", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                }
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
            PasswordTextBox.IsHitTestVisible = false;
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

            PasswordTextBox.IsHitTestVisible = true;
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
            PasswordTextBox.Text = _sellerToUpdate.Password;
            FullNameTextBox.Text = _sellerToUpdate.FullName;
            BirthDatePicker.SelectedDate = _sellerToUpdate.BirthDate.ToDateTime(TimeOnly.MinValue);
            IdentifyTextBox.Text = _sellerToUpdate.Cid;
            SpecificAddressTextBox.Text = _sellerToUpdate.SpecificAddress;
            StatusCheckBox.IsChecked = _sellerToUpdate.IsActive;
        }

    }
}
