// Card.cs
// Card class represents a playing card.
public class Card
{
    public string Face { get; init; }
    public string Suit { get; init; }

    // two-parameter constructor initializes card's face and suit
    public Card(string cardFace, string cardSuit)
    {
        Face = cardFace; // initialize face of card
        Suit = cardSuit; // initialize suit of card
    } // end two-parameter Card constructor

    // return string representation of Card
    public override string ToString()
    {
        return Face + " of " + Suit;
    } // end method ToString
} // end class Card

