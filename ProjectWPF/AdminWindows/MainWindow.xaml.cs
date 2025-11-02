using Repository.Models.user;
using Service.user;
using System.Windows;

namespace ProjectWPF.AdminWindows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private readonly NavigationWindow _navigationWindow;
        private readonly UserTokenService _userTokenService;
        private Admin? _loggedInAdmin;

        public MainWindow(NavigationWindow navigationWindow,
            UserTokenService userTokenService)
        {
            _navigationWindow = navigationWindow;
            _userTokenService = userTokenService;

            InitializeComponent();
        }

        public void SetLoggedInAdmin(Admin admin)
        {
            _loggedInAdmin = admin;
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                _userTokenService.DeleteUserToken(_loggedInAdmin!.Id);
                _navigationWindow.ShowWindowAndCloseCurrent<Login>(this);
            }
        }

        private void SellerManagerButton_Click(object sender, RoutedEventArgs e)
        {
            _navigationWindow.ShowDialog<SellerList>();
        }

        private void SellerRequestButton_Click(object sender, RoutedEventArgs e)
        {
            _navigationWindow.ShowDialog<SellerRequestList>();
        }
    }
}
