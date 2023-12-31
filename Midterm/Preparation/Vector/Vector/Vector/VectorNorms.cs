namespace VectorImplementation
{
    public class VectorNorms : Vector
    {
        public delegate double ComputeNorm(Vector v);

        public VectorNorms(int[] p, int i1, int num) : base(i1, num)
        {
            this.p = p;
            this.i1 = i1;
            count++;
        }

        public double FirstNorm(Vector v)
        {
            int total = 0;
            for (int i = 0; i < v.Length; i++)
            {
                total += v[i + v.First];
            }
            return Math.Abs(total);
        }

        public double SecondNorm(Vector v)
        {
            return v * v;
        }
    }
}
