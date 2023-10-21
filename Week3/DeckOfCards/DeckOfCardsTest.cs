// DeckOfCardsTest.cs
// Card shuffling and dealing application.

public class DeckOfCardsTest
{
    // execute application
    public static void Main(string[] args)
    {
        DeckOfCards myDeckOfCards = new DeckOfCards();
        myDeckOfCards.Shuffle(); // place Cards in random order

        // display all 52 Cards in the order in which they are dealt
        for (int i = 0; i < 52; i++)
        {
            Console.Write("{0,-19}", myDeckOfCards.DealCard());

            if ((i + 1) % 4 == 0)
                Console.WriteLine();
        }
    }
}

