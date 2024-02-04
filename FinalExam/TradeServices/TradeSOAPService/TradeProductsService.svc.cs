using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.Threading;

namespace TradeSOAPService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class TradeProducts : IOrderWService
    {
        private Dictionary<string, Product> products = new Dictionary<string, Product>();
        private Random rand = new Random();
        private readonly object lockObject = new object();
        private readonly object lockProducts = new object(); // For thread-safe access to products
        private readonly object lockFile = new object(); // For thread-safe file operations
        private const string ReorderFilePath = @"reorder.txt"; // Path to the reorder file

        public TradeProducts()
        {
            InitializeProducts();
            StartStockManagementThread();
        }

        private void InitializeProducts()
        {
            int productsCount = rand.Next(6, 12); // [6,11]

            for (int i = 0; i < productsCount; i++)
            {
                var product = new Product
                {
                    ID = "P-" + i.ToString().PadLeft(4, '0'),
                    ProductCategory = (Category)rand.Next(0, 3), // Assuming 3 categories
                    ReorderLevel = rand.Next(6, 13), // [6,12]
                    Qty = 12 // Default quantity
                };

                products[product.ID] = product;
            }
        }

        private void StartStockManagementThread()
        {
            Thread stockManagementThread = new Thread(() =>
            {
                while (true)
                {
                    lock (lockObject) // Ensure thread-safe access to products
                    {
                        foreach (var product in products.Values)
                        {
                            if (product.Qty < product.ReorderLevel)
                            {
                                int replenishAmount = rand.Next(product.ReorderLevel, product.ReorderLevel + 12);
                                product.Qty += replenishAmount;
                                string toPrint = "Replensihed " + product.ID.ToString() + ": New Qty = " + product.Qty.ToString();
                                Console.WriteLine(toPrint);
                            }
                        }
                    }
                    Thread.Sleep(1000);
                }
            })
            { IsBackground = true }; // Make the thread a background thread so it doesn't prevent the application from exiting
            stockManagementThread.Start();
        }

        public Product[] Retrieve()
        {
            lock (lockProducts)
            {
                Product[] productsArray = new Product[products.Values.Count];
                products.Values.CopyTo(productsArray, 0);
                return productsArray;
            }
        }

        public void Update(string sender, string productID, int qty)
        {
            lock (lockProducts)
            {
                if (products.ContainsKey(productID))
                {
                    Product product = products[productID];
                    lock (lockFile)
                    {
                        using (StreamWriter sw = new StreamWriter(ReorderFilePath, true)) // Append to the file
                        {
                            string logEntry = "Client: " + sender.ToString() + ", Product: " + products[productID] + ", Quantity: " + qty.ToString();
                            sw.WriteLine(logEntry);
                        }
                    }
                }
            }
        }
    }
}