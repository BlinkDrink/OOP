using Homework5.Concretes;
using Homework5.Interfaces;

namespace Homework5
{
    internal class TestPolymorphismProtected
    {
        public static void Main(string[] args)
        {
            TextBox[] textBoxArray = { new EditTextBox(), new RichTextBox(), new MultlineTextBox() };

            foreach (var textBox in textBoxArray)
            {
                // Uncomment below line to observe the compiler error
                // textBox.TypeText(); // Results in a compiler error because TypeText() is protected
                // As mentioned in the previous classes' implementation, we cannot access protected fields/methods unless
                // we do so in the derived class itself
            }
        }
    }
}