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
    public partial class SaveSeller : Window
    {

        private readonly Seller? _sellerToUpdate;
        private readonly SellerService _sellerService = new();
        private readonly AddressService _addressService = new();

        public SaveSeller()
        {
            InitializeComponent();
        }

        public SaveSeller(Seller? seller) : this()
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
                DeleteSellerButton.Visibility = Visibility.Hidden;
                CommuneWardComboBox.ItemsSource = new List<CommuneWard> { new() { Code = "default", Name = "Chọn Xã/Phường", ProvinceCityCode = "0" } };
                ProvinceCityComboBox.SelectedIndex = 0;
                CommuneWardComboBox.SelectedIndex = 0;
                StatusCheckBox.IsChecked = true;
            }
            else
            {
                Title = "Cập Nhật Người Bán";
                SaveSellerTitleTextBlock.Text = "Cập Nhật Người Bán";
                SaveSellerButton.Content = "Cập Nhật Người Bán";

                AutoFillSellerToUpdate();
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
                    _sellerToUpdate.CommuneWardCode = communneWard;
                    _sellerToUpdate.CommuneWard = null;
                    _sellerToUpdate.IsActive = isActive;
                    _sellerService.UpdateSeller(_sellerToUpdate);
                    MessageBox.Show("Cập nhật người bán thành công.", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                this.Close();
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn muốn đặt lại tất cả thay đổi?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) return;

            if (_sellerToUpdate == null)
            {
                EmailTextBox.Text = "";
                EmailTextBox.IsEnabled = true;
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
                AutoFillSellerToUpdate();
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

        private void AutoFillSellerToUpdate()
        {
            string? sellerProvinceCityCode = _sellerToUpdate!.CommuneWard?.ProvinceCityCode;
            ProvinceCityComboBox.SelectedValue = sellerProvinceCityCode;
            var communeWards = _addressService.GetCommuneWardsByProvinceCityCode(sellerProvinceCityCode!);
            communeWards.Insert(0, new() { Code = "default", Name = "Chọn Xã/Phường", ProvinceCityCode = "0" });
            CommuneWardComboBox.ItemsSource = communeWards;
            CommuneWardComboBox.SelectedValue = _sellerToUpdate.CommuneWardCode;

            EmailTextBox.Text = _sellerToUpdate.Email;
            EmailTextBox.IsEnabled = false;
            PasswordTextBox.Text = _sellerToUpdate.Password;
            FullNameTextBox.Text = _sellerToUpdate.FullName;
            BirthDatePicker.SelectedDate = _sellerToUpdate.BirthDate.ToDateTime(TimeOnly.MinValue);
            IdentifyTextBox.Text = _sellerToUpdate.Cid;
            SpecificAddressTextBox.Text = _sellerToUpdate.SpecificAddress;
            StatusCheckBox.IsChecked = _sellerToUpdate.IsActive;
        }
    }
}
