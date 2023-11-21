using Homework5.Interfaces;

namespace Homework5.Concretes
{
    public class EditTextBox : TextBox
    {
        protected override void TypeText()
        {
            Console.WriteLine(text);
        }

        public override void EditTextAllowed()
        {
            TypeText();
            Console.WriteLine(text);
            Console.WriteLine(baseText);
        }

        public override void EditTextDisAllowed()
        {
            TextBox baseClass = (TextBox)this;
            // Accessing protected members from anything but the derived classes is impossible
            // baseClass.TypeText(); <- this results in compile time error due to TypeText being protected

            // Protected members may be accessed only in the classes that derive the base class
            // baseClass.text = "New text"; // Uncommenting this line will result in a compiler error
            // baseClass.baseText = "New base text"; // Attempting to assign new values to protected members results in a compiler error
        }
    }
}
