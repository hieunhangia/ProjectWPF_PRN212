using Repository;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ProjectWPF.SellerWindows
{
    /// <summary>
    /// Interaction logic for ProductDetail.xaml
    /// </summary>
    public partial class ProductDetail : Window
    {
        public ProductDetail(Product product)
        {
            InitializeComponent();
            DataContext = product;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}