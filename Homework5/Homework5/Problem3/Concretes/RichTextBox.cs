namespace Problem3.Concretes
{
    public class RichTextBox : TextBox
    {
        protected override void UndoImplementation()
        {
            Console.WriteLine($"{GetType()}.Undo");
        }
    }
}
