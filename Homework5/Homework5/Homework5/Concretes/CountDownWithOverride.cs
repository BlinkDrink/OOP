namespace Homework5.Concretes
{
    public class CountDownWithOverride : Homework5.Concretes.CountDown
    {
        public override object Current => 16 - count;
        public override bool MoveNext() => count-- > 0;
        public override void Reset() => count = 0;
    }
}
