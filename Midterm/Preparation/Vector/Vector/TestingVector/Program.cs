using VectorImplementation;

namespace TestingVector
{
    class Program
    {
        static void Main()
        {
            Vector v1 = new Vector(3, 6); // Създаваме вектор v1, започващ от индекс 3 с дължина 6
            v1[3] = 0;
            v1[4] = 1;
            v1[5] = 2;
            v1[6] = 3;
            v1[7] = 4;
            v1[8] = 5;

            Console.WriteLine($"Vector v1: {v1}");

            double evalResult = v1.Evaluate(1);
            Console.WriteLine($"Evaluation at x = 1: {evalResult}");

            Polynomial derivative = v1.Differentiate();
            Console.WriteLine($"Derivative of v1: {((Vector)derivative).Evaluate(1)}");

            VectorNorms norms = new VectorNorms();
            double firstNorm = norms.FirstNorm(v1);
            Console.WriteLine($"First Norm of v1: {firstNorm}");

            double secondNorm = norms.SecondNorm(v1);
            Console.WriteLine($"Second Norm of v1: {secondNorm}");

            Console.WriteLine($"Active Vector objects: {Vector.Count}");

            double normResult = v1.Norm(vector => norms.SecondNorm(vector)); // Пример за използване на разширяващ метод

            Console.WriteLine($"Norm of the vector: {normResult}");
        }
    }
}