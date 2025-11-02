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
        private readonly UserTokenService _userTokenService;

        public Login(NavigationWindow navigationWindow,
            UserService userService,
            UserTokenService userTokenService)
        {
            _navigationWindow = navigationWindow;
            _userService = userService;
            _userTokenService = userTokenService;

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var user = _userTokenService.GetUserIfValidTokenExist();
            if (user != null)
            {
                NavigateUserToSpecificWindow(user);
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                User user = _userService.Login(EmailTextBox.Text, PasswordBox.Password);

                if(RememberMeCheckBox.IsChecked == true)
                {
                    _userTokenService.SaveUserToken(user.Id, DateTime.Now.AddDays(30));
                }
                
                NavigateUserToSpecificWindow(user);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void NavigateUserToSpecificWindow(User user)
        {
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
        }
    }
}