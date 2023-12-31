namespace VectorImplementation
{
    public interface Polynomial
    {
        double Evaluate(double x);
        Polynomial Differentiate();
    }
}