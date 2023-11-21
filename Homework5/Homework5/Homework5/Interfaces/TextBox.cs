namespace Homework5.Interfaces
{
    public abstract class TextBox
    {
        protected string text;
        protected string baseText;

        public string Text { get => text; private set { text = value; } }
        public string BaseText { get => baseText; private set { baseText = value; } }

        #region Constructors
        public TextBox()
        {
            Text = $"{GetType()}:Type text";
            BaseText = $"{GetType()}:Type baseText";
        }
        #endregion

        #region Methods

        protected void TypeText() { Console.WriteLine(text); }

        public abstract void EditTextAllowed();

        public abstract void EditTextDisAllowed();

        #endregion
    }
}
