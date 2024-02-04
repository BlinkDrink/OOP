using System.Windows;
using System.Windows.Controls;

namespace TradeServices.UserControls
{
    public partial class OrderProduct : UserControl
    {
        public OrderProduct()
        {
            InitializeComponent();
        }

        public ComboBox ProductComboBox => cboProduct;

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            txtQty.Text = "0";
        }
    }
}
