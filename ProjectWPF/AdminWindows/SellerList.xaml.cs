using Repository.Models.user;
using Service.user;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

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

        private void SellerListView_Loaded(object sender, RoutedEventArgs e)
        {
            double totalWidth = SellerListView.ActualWidth;
            ListViewEmailColumn.Width = totalWidth * 0.36;
            ListViewFullNameColumn.Width = totalWidth * 0.36;
            ListViewStatusColumn.Width = totalWidth * 0.14;
            ListViewActionColumn.Width = totalWidth * 0.14;
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
            SellerListView.ItemsSource = _sellerService.GetAllSellers();
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

        private bool isSort = true;
        private void ListViewHeader_Click(object sender, RoutedEventArgs e)
        {
            var pathBinding = ((Binding)((GridViewColumnHeader)sender).Column.DisplayMemberBinding).Path.Path;

            var view = CollectionViewSource.GetDefaultView(SellerListView.ItemsSource);
            view.SortDescriptions.Clear();
            if (isSort)
            {
                view.SortDescriptions.Add(new SortDescription(pathBinding, ListSortDirection.Ascending));
            }
            else
            {
                view.SortDescriptions.Add(new SortDescription(pathBinding, ListSortDirection.Descending));
            }
            isSort = !isSort;
        }

        private void EmailSearchTextBox_TextChanged(object sender, TextChangedEventArgs e) => DoSearching();

        private void FullNameSearchTextBox_TextChanged(object sender, TextChangedEventArgs e) => DoSearching();

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
            SellerListView.ItemsSource = sellers;
        }
    }
}
