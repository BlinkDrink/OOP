namespace CodeChangeLib
{
    public class CodeChangeEventArgs : EventArgs
    {
        public List<int> Code { get; init; }

        public CodeChangeEventArgs(List<int> ints)
        {
            Code = ints.ToList();
        }
    }

}