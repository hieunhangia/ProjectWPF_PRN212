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

    public class BooleanToStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isActive)
            {
                return isActive ? "Hoạt động" : "Không hoạt động";
            }
            return "Không xác định";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ExpiryStatusConverter : IValueConverter
    {
        public static readonly ExpiryStatusConverter Instance = new ExpiryStatusConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime expiryDate)
            {
                return expiryDate < DateTime.Now ? "Hết hạn" : "Còn hạn";
            }
            return "Không xác định";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class IsExpiredConverter : IValueConverter
    {
        public static readonly IsExpiredConverter Instance = new IsExpiredConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime expiryDate)
            {
                return expiryDate < DateTime.Now;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}