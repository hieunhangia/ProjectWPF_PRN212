using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectWPF.SellerWindows;
using Service.seller_request;
using System.Windows;
using System.Windows.Controls;

namespace ProjectWPF.AdminWindows
{
    /// <summary>
    /// Interaction logic for SellerRequest.xaml
    /// </summary>
    public partial class SellerRequestList : Window
    {
        private readonly NavigationWindow _navigationWindow;
        private readonly SellerRequestService _sellerRequestService;
        private readonly IServiceProvider _serviceProvider;

        public SellerRequestList(NavigationWindow navigationWindow,
                                SellerRequestService sellerRequestService,
                                IServiceProvider serviceProvider)
        {
            _navigationWindow = navigationWindow;
            _sellerRequestService = sellerRequestService;
            InitializeComponent();
            InitRequestList();
            _serviceProvider = serviceProvider;
        }

        private void InitRequestList()
        {
            var context = _sellerRequestService.getSellerRequestsContext();

            RequestsDataGrid.ItemsSource = context;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            var button = sender as Button;
            if (button?.Tag is long id)
            {
                var sellerRequestDetailFactory = _serviceProvider.GetRequiredService<Func<long, SellerRequestDetail>>();
                var detailWindow = sellerRequestDetailFactory(id);
                detailWindow.Show();
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            InitRequestList();
            MessageBox.Show("Danh sách đã được làm mới", "Thông báo",
                           MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
