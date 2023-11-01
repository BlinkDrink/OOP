namespace Problem2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Prompt user for input
            double x;
            Console.WriteLine("Enter x:");

            // Read user input
            if (!double.TryParse(Console.ReadLine(), out x))
            {
                Console.WriteLine("Wrong input.");
                Console.ReadLine();
                return;
            }

            // Print the comparison between our implementation of cos and the integrated cos implementation
            Console.WriteLine($"Approximate cos({x}): {ComputeCos(x)}");
            Console.WriteLine($"Accurate cos({x}): {Math.Cos(x)}");
        }

        /// <summary>
        /// ComputeCos computes the cosine of x using sum approximation
        /// </summary>
        /// <param name="x">argument of cos()</param>
        /// <returns></returns>
        static double ComputeCos(double x)
        {
            // Declaration of vars
            double accuracy = 0.00000001; // accuracy of computation
            double cosValue = 0; // the cosine of x to be returned
            double term = 1.0; // current member of the sum
            int count = 0; // keeps count of the index of the current member of the sum

            // processing
            do
            {
                count += 2;
                term = -term * x * x / count * (count - 1);
                cosValue += term;
            } while (Math.Abs(term) > accuracy);

            return cosValue;
        }

    }
}