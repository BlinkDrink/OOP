using Problem3.Interfaces;

namespace Problem3.Concretes
{
    public abstract class TextBox : IUndoable
    {
        protected abstract void UndoImplementation();

        public void Undo()
        {
            UndoImplementation();
        }
    }
}
