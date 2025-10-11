using Microsoft.Extensions.DependencyInjection;
using Repository.Models.user;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IServiceProvider _serviceProvider;

        private Admin? _loggedInAdmin;

        public MainWindow(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

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
                _serviceProvider.GetRequiredService<Login>().Show();
                this.Close();
            }
        }

        private void SellerManagerButton_Click(object sender, RoutedEventArgs e)
        {
            _serviceProvider.GetRequiredService<SellerList>().ShowDialog();
        }

        private void SellerRequestButton_Click(object sender, RoutedEventArgs e)
        {
            _serviceProvider.GetRequiredService<SellerRequest>().ShowDialog();
        }
    }
}
