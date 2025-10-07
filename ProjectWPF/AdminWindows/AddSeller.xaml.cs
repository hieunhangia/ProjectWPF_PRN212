using Repository;
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
    public partial class AddSeller : Window
    {

        private readonly AddressService _addressService = new();

        public AddSeller()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var provinceCities = _addressService.GetAllProvinceCities();
            provinceCities.Insert(0, new() { Code = "default", Name = "Chọn Tỉnh/Thành Phố" });
            ProvinceCityComboBox.ItemsSource = provinceCities;
            ProvinceCityComboBox.SelectedIndex = 0;

            CommuneWardComboBox.ItemsSource = new List<CommuneWard> { new() { Code = "default", Name = "Chọn Xã/Phường" , ProvinceCityCode = "0"} };
            CommuneWardComboBox.SelectedIndex = 0;
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

        private void AddSellerButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text;
            string password = PasswordTextBox.Text;
            string fullName = FullNameTextBox.Text;
            DateOnly? birthDate = BirthDatePicker.SelectedDate.HasValue ? DateOnly.FromDateTime(BirthDatePicker.SelectedDate.Value) : null;
            string identify = IdentifyTextBox.Text;
            string provinceCity = (string)ProvinceCityComboBox.SelectedValue;
            string communneWard = (string)CommuneWardComboBox.SelectedValue;
            string specificAddress = SpecificAddressTextBox.Text;

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

                SellerService sellerService = new();
                if (sellerService.IsEmailExists(email))
                {
                    MessageBox.Show("Email đã tồn tại.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (sellerService.IsIdentifyExists(identify))
                {
                    MessageBox.Show("CMND/CCCD đã tồn tại.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                Seller seller = new()
                {
                    Email = email,
                    Password = password,
                    IsActive = true,
                    FullName = fullName,
                    BirthDate = birthDate.Value,
                    Cid = identify,
                    SpecificAddress = specificAddress,
                    CommuneWardCode = communneWard
                };
                sellerService.AddSeller(seller);
                MessageBox.Show("Thêm người bán thành công.", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
        }
    }
}
