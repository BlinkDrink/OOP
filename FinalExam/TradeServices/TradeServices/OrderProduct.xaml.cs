using System.Windows;
using System.Windows.Controls;
using TradeServices.DelegatesAndEventArgs;

namespace TradeServices.UserControls
{
    public partial class OrderProduct : UserControl
    {
        public event OrderHandler OrderPlaced;

        public OrderProduct()
        {
            InitializeComponent();
        }

        public ComboBox ProductComboBox => cboProduct;

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            txtQty.Text = "0";
        }

        private void Order_Click(object sender, RoutedEventArgs e)
        {
            var productCode = ProductComboBox.SelectedItem.ToString(); // Assuming the ComboBox is populated with product codes
            var quantity = int.TryParse(txtQty.Text, out int qty) ? qty : 0; // Safely parse the quantity
            OrderPlaced?.Invoke(this, new OrderEventArgs(productCode, quantity)); // Raise the OrderPlaced event
        }
    }
}
