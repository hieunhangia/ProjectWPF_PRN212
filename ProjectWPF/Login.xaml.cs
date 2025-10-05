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

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            UserService userService = new();

            var user = userService.Login(emailTextBox.Text, passwordBox.Password);

            if (user != null)
            {
                MessageBox.Show("Chuẩn cmnr", "Chuẩn cmnr", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                MessageBox.Show("Sai cmnr", "Sai cmnr", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}