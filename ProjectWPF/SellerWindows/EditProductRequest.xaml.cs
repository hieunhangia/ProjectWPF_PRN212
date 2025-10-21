using Service.seller_request;
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
    /// Interaction logic for EditProductRequest.xaml
    /// </summary>
    public partial class EditProductRequest : Window
    {

        public EditProductRequest(long productId, SellerRequestService sellerRequestService)
        {
            InitializeComponent();
        }
    }
}
