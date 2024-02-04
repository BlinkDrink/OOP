using System.Windows;
using System.Windows.Controls;

namespace ProductClientApp
{
    /// <summary>
    /// Interaction logic for OrderClient.xaml
    /// </summary>
    public partial class OrderClient : Window
    {
        private UserControl orderProductInstance;
        private IOrderWService client;

        public OrderClient()
        {
            InitializeComponent();

            Random rand = new Random();
            Title = $"Order Client {rand.Next(1, 1001)}";

            orderProductInstance = orderProduct;

            client = new TradeSOAPService.TradeProducts();
            PopulateProductsComboBox();
        }

        private void PopulateProductsComboBox()
        {
            Product[] products = client.Retrieve();
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() =>
                 {
                     foreach (var product in products)
                     {
                         orderProduct.ProductComboBox.Items.Add(product.ID);
                     }
                 });
            }
            else
            {
                foreach (var product in products)
                {
                    orderProduct.ProductComboBox.Items.Add(product.ID);
                }
            }
        }
    }
}
