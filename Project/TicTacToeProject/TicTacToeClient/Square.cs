using System.Windows.Controls;

// the representation of a square in a tic-tac-toe grid
public class Square
{
    private Panel panel; // GUI Panel that represents this Square
    private char mark; // player’s mark on this Square (if any)
    private int location; // location on the board of this Square

    // constructor
    public Square(Panel newPanel, char newMark, int newLocation)
    {
        panel = newPanel;
        mark = newMark;
        location = newLocation;
    } // end constructor

    // property SquarePanel; the panel which the square represents
    public Panel SquarePanel
    {
        get
        {
            return panel;
        } // end get
    } // end property SquarePanel 

    // property Mark; the mark on the square
    public char Mark
    {
        get
        {
            return mark;
        } // end get
        set
        {
            mark = value;
        } // end set
    } // end property Mark

    // property Location; the square's location on the board
    public int Location
    {
        get
        {
            return location;
        } // end get
    } // end property Location
} // end class Square 