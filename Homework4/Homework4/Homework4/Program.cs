namespace Homework4
{
    internal class Program
    {
        public static List<Product> products = new List<Product> {
        new Product("Electric sander", Type.M, new List<int> { 99, 82, 81, 79 },  157.98m),
        new Product ("Power saw", Type.M, new List < int > { 99, 86, 90, 94 }, 157.98m) ,
        new Product ( "Sledge hammer", Type.F, new List<int> { 93, 92, 80, 87 },  21.50m ),
        new Product ( "Hammer", Type.M, new List < int > { 97, 89, 85, 82 }, 11.99m ),
        new Product ( "Lawn mower", Type.F, new List < int > { 35, 72, 91, 70 }, 139.50m ),
        new Product ( "Screwdriver", Type.F, new List < int > { 88, 94, 65, 91 }, 56.99m ),
        new Product ( "Jig saw", Type.M, new List < int > { 75, 84, 91, 39 }, 11.00m ),
        new Product ( "Wrench", Type.F, new List < int > { 97, 92, 81, 60 }, 17.50m ),
        new Product ( "Sledge hammer", Type.M, new List < int > { 75, 84, 91, 39 }, 21.50m ),
        new Product ( "Hammer", Type.F, new List < int > { 94, 92, 91, 91 }, 11.99m ),
        new Product ( "Lawn mower", Type.M, new List <int> { 96, 85, 91, 60 }, 179.50m ),
        new Product ( "Screwdriver", Type.M, new List < int > { 96, 85, 51, 30 }, 66.99m ),
        };

        public static void GroupByCategoryCountDescending()
        {
            var query = products.GroupBy(x => x.Category, (category, products) => new { Key = category, Count = products.Count() }).OrderByDescending(x => x.Count);

            foreach (var result in query)
            {
                Console.WriteLine($"Category group: {result.Key}\n\t\tNumber of products of Type {result.Count} in this group: {result.Count}");
            }
        }

        public static void GroupByQtrAndProductPriceAvg()
        {
            var query = products.GroupBy(x => x.Quarter).Select(group => new { Quarter = group.Key, AvgPrice = group.Average(product => product.Price) }).OrderBy(x => x.Quarter);

            foreach (var group in query)
            {
                Console.WriteLine($"Quarter group: {group.Quarter}\n\tAverage price per Quarter: ${group.AvgPrice:F2}");
            }
        }

        public static void GroupByQtrCategoryWeeklySum()
        {
            var result = products
            .GroupBy(product => product.Quarter)
            .OrderBy(quarterGroup => quarterGroup.Key)
            .Select(quarterGroup => new
            {
                Quarter = quarterGroup.Key,
                Categories = quarterGroup
                    .GroupBy(product => product.Category)
                    .Select(categoryGroup => new
                    {
                        Category = categoryGroup.Key,
                        Products = categoryGroup
                            .Select(product => new
                            {
                                Description = product.Description,
                                TotalWeeklyPurchases = product.WeeklyPurchases.Sum()
                            })
                    }).OrderByDescending(x => x.Category)
            });

            // Display the results
            foreach (var quarterGroup in result)
            {
                Console.WriteLine($"Quarter group: {quarterGroup.Quarter}");
                foreach (var categoryGroup in quarterGroup.Categories)
                {
                    Console.WriteLine($"\tCategory group: {categoryGroup.Category}");
                    foreach (var product in categoryGroup.Products)
                    {
                        Console.WriteLine($"\t\t({product.Description}, {product.TotalWeeklyPurchases})");
                    }
                }
            }
        }

        public static void GroupByQtrCategoryAndProducts()
        {
            var result = products.GroupBy(x => x.Quarter).Select(quarter => new
            {
                Quarter = quarter.Key,
                Categories = quarter.GroupBy(x => x.Category).Select(category => new
                {
                    Category = category.Key,
                    Products = category.Select(product => new { ToPrint = product.ToString() })
                }).OrderBy(x => x.Category)
            }).OrderBy(x => x.Quarter);

            foreach (var quarterGroup in result)
            {
                Console.WriteLine($"Quarter group: {quarterGroup.Quarter}");
                foreach (var categoryGroup in quarterGroup.Categories)
                {
                    Console.WriteLine($"\tCategory group: {categoryGroup.Category}");
                    foreach (var product in categoryGroup.Products)
                    {
                        Console.WriteLine($"\t\t{product.ToPrint}");
                    }
                }
            }
        }

        public static void GroupByQtrMinMaxPrice()
        {
            var result = products.GroupBy(x => x.Quarter).Select(quarter => new
            {
                Quarter = quarter.Key,
                Min = quarter.Min(x => x.Price),
                Max = quarter.Max(x => x.Price)
            }).OrderBy(x => x.Quarter);

            foreach (var quarterGroup in result)
            {
                Console.WriteLine($"Quarter group: {quarterGroup.Quarter}");
                Console.WriteLine($"\t\tMin price per Quarter: {quarterGroup.Min}");
                Console.WriteLine($"\t\tMax price per Quarter: {quarterGroup.Max}");
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("\nGroupByCategoryCountDescending()\n");
            GroupByCategoryCountDescending();
            Console.WriteLine("\nGroupByQtrAndProductPriceAvg()\n");
            GroupByQtrAndProductPriceAvg();
            Console.WriteLine("\nGroupByQtrCategoryWeeklySum()\n");
            GroupByQtrCategoryWeeklySum();
            Console.WriteLine("\nGroupByQtrCategoryAndProducts()\n");
            GroupByQtrCategoryAndProducts();
            Console.WriteLine("\nGroupByQtrMinMaxPrice()\n");
            GroupByQtrMinMaxPrice();
        }
    }
}