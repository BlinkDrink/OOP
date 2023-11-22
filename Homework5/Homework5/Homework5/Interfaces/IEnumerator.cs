namespace Homework5.Interfaces
{
    /// <summary>
    /// Interface with default implementation
    /// </summary>
    public interface IEnumerator
    {
        bool MoveNext() => throw new NotImplementedException();
        object Current => throw new NotImplementedException();
        void Reset() => throw new NotImplementedException();
    }
}
