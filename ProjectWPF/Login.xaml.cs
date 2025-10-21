using Repository.Models.user;
using Service.user;
using System.Windows;
using System.Windows.Controls;

namespace ProjectWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Login : Window
    {
        private readonly NavigationWindow _navigationWindow;
        private readonly UserService _userService;

        public Login(NavigationWindow navigationWindow,
            UserService userService)
        {
            _navigationWindow = navigationWindow;
            _userService = userService;

            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                User user = _userService.Login(EmailTextBox.Text, PasswordBox.Password);

                if (user is Admin admin)
                {
                    var adminWindow = _navigationWindow.GetWindow<AdminWindows.MainWindow>();
                    adminWindow.SetLoggedInAdmin(admin);
                    adminWindow.Show();
                    this.Close();
                }
                else if (user is Seller seller)
                {
                    var sellerWindow = _navigationWindow.GetWindow<SellerWindows.MainWindow>();
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