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
            if (emailTextBox.Text != "quetoi36" || passwordBox.Password != "suthanhhoa")
            {
                MessageBox.Show("Sai cmnr");
            }
            else
            {
                MessageBox.Show("Chuẩn cmnr");
            }
        }
    }
}