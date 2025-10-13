using Microsoft.Extensions.DependencyInjection;
using Repository.Models.user;
using Service.user;
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
        private readonly NavigationWindow _navigationWindow;
        private readonly SellerService _sellerService;

        public SellerList(NavigationWindow navigationWindow,
            SellerService sellerService)
        {
            _navigationWindow = navigationWindow;
            _sellerService = sellerService;

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitWindow();
        }

        private void AddSellerButton_Click(object sender, RoutedEventArgs e)
        {
            _navigationWindow.ShowDialog<SellerForm>();

            InitWindow();
        }

        private void ShowDetailsSellerHandler(object sender, MouseButtonEventArgs e)
        {
            var textBlock = sender as TextBlock;
            var seller = textBlock?.DataContext as Seller;

            var _sellerForm = _navigationWindow.GetWindow<SellerForm>();
            _sellerForm.SetSellerToUpdate(seller!);
            _sellerForm.ShowDialog();

            InitWindow();
        }

        private void InitWindow()
        {
            SellerDataGrid.ItemsSource = _sellerService.GetAllSellers();
            EmailSearchTextBox.Text = "";
            FullNameSearchTextBox.Text = "";

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

        private void SearchButton_Click(object sender, RoutedEventArgs e) => DoSearching();

        private void StatusSearchTextBox_SelectionChanged(object sender, SelectionChangedEventArgs e) => DoSearching();

        private void DoSearching()
        {
            string email = EmailSearchTextBox.Text.Trim().ToLower();
            string fullName = FullNameSearchTextBox.Text.Trim().ToLower();
            int status = (int)StatusSearchTextBox.SelectedValue;
            var sellers = _sellerService.GetSellersByCondition(s =>
                (string.IsNullOrEmpty(email) || s.Email!.ToLower().Contains(email.ToLower()))
                && (string.IsNullOrEmpty(fullName) || s.FullName!.ToLower().Contains(fullName.ToLower()))
                && (status == 0 || (status == 1 && s.IsActive) || (status == 2 && !s.IsActive))
            );
            SellerDataGrid.ItemsSource = sellers;
        }
    }
}
