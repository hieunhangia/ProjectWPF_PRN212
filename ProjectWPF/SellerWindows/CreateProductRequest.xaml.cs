using Service.product;
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

namespace ProjectWPF.SellerWindows
{
    /// <summary>
    /// Interaction logic for CreateProductRequest.xaml
    /// </summary>
    public partial class CreateProductRequest : Window
    {
        private readonly SellerService _sellerService;
        private readonly ProductService _productService;
        public CreateProductRequest(SellerService sellerService,ProductService productService)
        {
            _sellerService = sellerService;
            _productService = productService;
            InitializeComponent();
            InitData();
        }

        private void InitData()
        {
            UnitComboBox.ItemsSource = _productService.GetProductUnits();
            UnitComboBox.SelectedIndex = 0;
        }
    }
}
