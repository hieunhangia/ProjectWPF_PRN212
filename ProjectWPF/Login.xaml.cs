using Repository.Models.user;
using Service;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjectWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            UserService userService = new();

            var user = userService.Login(emailTextBox.Text, passwordBox.Password);

            if (user != null)
            {
                if (user.IsActive)
                {
                    if (user is Admin admin)
                    {
                        new AdminWindows.MainWindow(admin).Show();
                        this.Close();
                    }
                    else if (user is Seller seller)
                    {
                        new SellerWindows.MainWindow(seller).Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Loại tài khoản không hợp lệ.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Tài khoản của bạn đã bị vô hiệu hóa.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                
            }
            else
            {
                MessageBox.Show("Tên tài khoản hoặc mật khẩu không đúng", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}