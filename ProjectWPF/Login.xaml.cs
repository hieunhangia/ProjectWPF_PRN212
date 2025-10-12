using Controller;
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
        private readonly UserController _userController;

        public Login(IServiceProvider serviceProvider,
            UserController userController)
        {
            _serviceProvider = serviceProvider;
            _userController = userController;

            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                User user = _userController.Login(EmailTextBox.Text, PasswordBox.Password);
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
                    MessageBox.Show("Loại người dùng không xác định.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }
}