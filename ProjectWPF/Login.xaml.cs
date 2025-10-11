using Microsoft.Extensions.DependencyInjection;
using Repository.Models.user;
using Service.user;
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
        private readonly IServiceProvider _serviceProvider;
        private readonly UserService _userService;

        public Login(IServiceProvider serviceProvider,
            UserService userService)
        {
            _serviceProvider = serviceProvider;
            _userService = userService;

            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var user = _userService.Login(emailTextBox.Text, passwordBox.Password);

            if (user != null)
            {
                if (user.IsActive)
                {
                    if (user is Admin admin)
                    {
                        var adminWindow = _serviceProvider.GetRequiredService<AdminWindows.MainWindow>();
                        adminWindow.SetLoggedInAdmin(admin);
                        adminWindow.Show();
                        this.Close();
                    }
                    else if (user is Seller seller)
                    {
                        var sellerWindow = _serviceProvider.GetRequiredService<SellerWindows.MainWindow>();
                        sellerWindow.SetLoggedInSeller(seller);
                        sellerWindow.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Loại tài khoản không hợp lệ.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Tài khoản của bạn đã bị khoá.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                
            }
            else
            {
                MessageBox.Show("Tên tài khoản hoặc mật khẩu không đúng", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}