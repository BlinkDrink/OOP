using Homework5.Interfaces;

namespace Homework5.Concretes
{
    public class MultlineTextBox : RichTextBox
    {
        protected override void TypeText()
        {
            Console.WriteLine(text);
        }

        public override void EditTextAllowed()
        {
            Console.WriteLine(text);
            Console.WriteLine(baseText);
        }

        public override void EditTextDisAllowed()
        {
            TextBox baseClass = (TextBox)this;
            // Protected methods may only be accessed from classes that derive the base class
            // baseClass.TypeText(); // Uncommenting this line will result in a compiler error

            // Protected data members may only be accessed from classes that derive the base class
            // baseClass.text = "New text"; // Uncommenting this line will result in a compiler error
            // baseClass.baseText = "New base text"; // Uncommenting this line will result in a compiler error
        }
    }
}
