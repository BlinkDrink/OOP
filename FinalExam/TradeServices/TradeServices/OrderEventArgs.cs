namespace TradeServices.DelegatesAndEventArgs
{
    /// <summary>
    /// Provides data for the Order event.
    /// </summary>
    public class OrderEventArgs : EventArgs
    {
        public string ProductCode { get; private set; }
        public int Quantity { get; private set; }

        public OrderEventArgs(string productCode, int quantity)
        {
            ProductCode = productCode;
            Quantity = quantity;
        }
    }
}

namespace TradeServices.DelegatesAndEventArgs
{
    /// <summary>
    /// Represents the method that will handle the Order event.
    /// </summary>
    public delegate void OrderHandler(object sender, OrderEventArgs e);
}
