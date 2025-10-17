using Microsoft.Extensions.DependencyInjection;
using Repository;
using Repository.Models.user;
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
using TableDependency.SqlClient.Base.Enums;
using TableDependency.SqlClient.Base.EventArgs;

namespace ProjectWPF.SellerWindows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly NavigationWindow _navigationWindow;

        private Seller? _loggedInSeller;

        private readonly UserChangeListener _userChangeListener;

        public MainWindow(NavigationWindow navigationWindow,
            UserChangeListener userChangeListener)
        {
            _navigationWindow = navigationWindow;

            _userChangeListener = userChangeListener;
            _userChangeListener.StartListening(OnUserStatusChanged);

            InitializeComponent();
        }

        private void OnUserStatusChanged(object sender, RecordChangedEventArgs<User> e)
        {
            if (e.ChangeType == ChangeType.Update)
            {
                var oldUser = e.EntityOldValues;
                var changedUser = e.Entity;
                if (oldUser.IsActive && !changedUser.IsActive && oldUser.Id == _loggedInSeller!.Id)
                {
                    Dispatcher.Invoke(() =>
                    {
                        _navigationWindow.ShowWindowAndCloseCurrent<Login>(this);
                        MessageBox.Show("Tài khoản của bạn đã bị khoá.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                    });
                }
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _userChangeListener.StopListening();
        }

        public void SetLoggedInSeller(Seller seller)
        {
            _loggedInSeller = seller;
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                _navigationWindow.ShowWindowAndCloseCurrent<Login>(this);
            }
        }

        private void AiSupporterButton_Click(object sender, RoutedEventArgs e)
        {
            _navigationWindow.ShowDialog<AiSupporter>();
        }

        private void CreateProductRequestButton_Click(object sender, RoutedEventArgs e)
        {
            _navigationWindow.ShowWindowAndCloseCurrent<CreateProductRequest>(this);
        }
    }
}
