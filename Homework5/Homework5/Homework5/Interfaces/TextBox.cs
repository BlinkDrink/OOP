namespace Homework5.Interfaces
{
    public abstract class TextBox
    {
        #region Private Fields
        protected string? text;
        protected string? baseText;
        #endregion

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
