namespace TradeServices
{
    /// <summary>
    /// Represents a product with a unique ID, category, quantity, and reorder level.
    /// </summary>
    public class Product
    {
        private static int _idCounter = 1000; // Static counter to generate unique ID numbers
        public string ID { get; private set; }
        public Category ProductCategory { get; set; }
        public int Qty { get; set; }
        public int ReorderLevel { get; set; }

        /// <summary>
        /// General use constructor to create a new product instance.
        /// </summary>
        /// <param name="productCategory">Product category.</param>
        /// <param name="qty">Available quantity.</param>
        /// <param name="reorderLevel">Reorder level threshold.</param>
        public Product(Category productCategory, int qty, int reorderLevel)
        {
            ID = $"P-{_idCounter++.ToString().PadLeft(4, '0')}";
            ProductCategory = productCategory;
            Qty = qty;
            ReorderLevel = reorderLevel;
        }

        /// <summary>
        /// Copy constructor to create a new product instance from an existing one.
        /// </summary>
        /// <param name="other">The product instance to copy from.</param>
        public Product(Product other)
        {
            ID = other.ID;
            ProductCategory = other.ProductCategory;
            Qty = other.Qty;
            ReorderLevel = other.ReorderLevel;
        }

        /// <summary>
        /// Provides a string representation of the product instance.
        /// </summary>
        /// <returns>A string detailing the product's properties.</returns>
        public override string ToString()
        {
            return $"ID: {ID}, Category: {ProductCategory}, Quantity: {Qty}, Reorder Level: {ReorderLevel}";
        }
    }
}
