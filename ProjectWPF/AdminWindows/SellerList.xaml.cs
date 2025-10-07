using Repository;
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
    /// Interaction logic for SellerList.xaml
    /// </summary>
    public partial class SellerList : Window
    {
        private readonly SellerService _sellerService = new();

        public SellerList()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SellerDataGrid.ItemsSource = _sellerService.GetAllSellers();

            var statuses = new List<Tuple<int, string>>
            {
                new (0, "Tất Cả"),
                new (1, "Đang Mở Khoá"),
                new (2, "Đã Khoá")
            };
            StatusSearchTextBox.ItemsSource = statuses;
            StatusSearchTextBox.DisplayMemberPath = "Item2";
            StatusSearchTextBox.SelectedValuePath = "Item1";
            StatusSearchTextBox.SelectedIndex = 0;
        }

        private void AddSellerButton_Click(object sender, RoutedEventArgs e)
        {
            new AddSeller().ShowDialog();

            SellerDataGrid.ItemsSource = _sellerService.GetAllSellers();
        }

        private void ViewDetailHandler(object sender, MouseButtonEventArgs e)
        {
            var textBlock = sender as TextBlock;
            var seller = textBlock?.DataContext as Seller;
            MessageBox.Show("Chức năng đang được phát triển" + seller?.Email);
        }

        private void UpdateSellerHandler(object sender, MouseButtonEventArgs e)
        {
            var textBlock = sender as TextBlock;
            var seller = textBlock?.DataContext as Seller;
            new UpdateSeller(seller).ShowDialog();

            SellerDataGrid.ItemsSource = _sellerService.GetAllSellers();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailSearchTextBox.Text.Trim().ToLower();
            string fullName = FullNameSearchTextBox.Text.Trim().ToLower();
            int status = (int)StatusSearchTextBox.SelectedValue;
            var sellers = _sellerService.GetSellersByCondition(s =>
                (string.IsNullOrEmpty(email) || s.Email.ToLower().Contains(email))
                && (string.IsNullOrEmpty(fullName) || s.FullName.ToLower().Contains(fullName))
                && (status == 0 || (status == 1 && s.IsActive) || (status == 2 && !s.IsActive))
            );
            SellerDataGrid.ItemsSource = sellers;
        }
    }
}
