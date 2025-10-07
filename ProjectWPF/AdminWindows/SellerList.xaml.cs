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
        public SellerList()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SellerService sellerService = new();
            var sellers = sellerService.GetAllSellers();
            SellerDataGrid.ItemsSource = sellers;
        }
        private void AddSellerButton_Click(object sender, RoutedEventArgs e)
        {
            new AddSeller().ShowDialog();
        }

        private void UpdateSellerHandler(object sender, MouseButtonEventArgs e)
        {
            var textBlock = sender as TextBlock;
            var seller = textBlock?.DataContext as Seller;
            new UpdateSeller(seller).ShowDialog();
        }
    }
}
