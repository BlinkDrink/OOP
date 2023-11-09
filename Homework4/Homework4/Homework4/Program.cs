namespace Homework4
{
    internal class Program
    {
        static List<Product> products = new List<Product> {
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
        new Product ( "Lawn mower", Type.M, new List < int > { 96, 85, 91, 60 }, 179.50m ),
        new Product ( "Screwdriver", Type.M, new List < int > { 96, 85, 51, 30 }, 66.99m ),
        };


        static void Main(string[] args)
        {
            Console.WriteLine("");
        }
    }
}