namespace Homework5.Interfaces
{
    public abstract class TextBox
    {
        protected string? text;
        protected string? baseText;

        #region Constructors
        public TextBox()
        {
            text = $"{GetType()}:Type text";
            baseText = $"{GetType()}:Type baseText";
        }
        #endregion

        #region Methods

        protected abstract void TypeText();

        public abstract void EditTextAllowed();

        public abstract void EditTextDisAllowed();

        #endregion
    }
}
