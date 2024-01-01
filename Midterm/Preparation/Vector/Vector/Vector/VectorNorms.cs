namespace VectorImplementation
{
    public class VectorNorms
    {
        public delegate double ComputeNorm(Vector v);

        public double FirstNorm(Vector v)
        {
            double total = 0;
            for (int i = v.First; i < v.Last; i++)
            {
                total += Math.Abs(v[i]);
            }
            return total;
        }

        public double SecondNorm(Vector v)
        {
            return v * v;
        }
    }

}
