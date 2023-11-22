using Problem3.Interfaces;

namespace Problem3.Concretes
{
    public class MultilineRichTextBox : RichTextBox
    {
        protected override void UndoImplementation()
        {
            Console.WriteLine($"{GetType()}.Undo");
        }

        public void PolyTest()
        {
            TextBox richTextBox = new RichTextBox();
            TextBox multilineRichTextBox = new MultilineRichTextBox();

            ((IUndoable)richTextBox).Undo(); // Prints Problem3.Concretes.RichTextBox.Undo
            ((IUndoable)multilineRichTextBox).Undo(); // Prints Problem3.Concretes.MultilineRichTextBox.Undo
            // On the console we can observe that no matter the fact that both richTextBox and multilineRichTextBox are
            // upcasted, both of them know which implementation to look for when .Undo() is called. This is possible because
            // TextBox is an abstract class implementing the IUndoable interface with an abstract method UndoImplementation().
            // The Undo() method in TextBox calls the UndoImplementation() method.
            // RichTextBox and MultilineRichTextBox override the UndoImplementation() method to print the type of the class followed by .Undo.
            //// MultilineRichTextBox contains the PolyTest() method to demonstrate polymorphic behavior by creating instances of RichTextBox and MultilineRichTextBox, upcasting them to IUndoable, and calling the Undo() method, which ultimately invokes the overridden UndoImplementation() method, printing the respective type followed by .Undo.
        }
    }
}
