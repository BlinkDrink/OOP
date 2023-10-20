// Card.cs
// Card class represents a playing card.
public class Card
{
    private string face; // face of card ("Ace", "Deuce", ...)
    private string suit; // suit of card ("Hearts", "Diamonds", ...)

    // two-parameter constructor initializes card's face and suit
    public Card(string cardFace, string cardSuit)
    {
        face = cardFace; // initialize face of card
        suit = cardSuit; // initialize suit of card
    } // end two-parameter Card constructor

    // return string representation of Card
    public override string ToString()
    {
        return face + " of " + suit;
    } // end method ToString
} // end class Card

