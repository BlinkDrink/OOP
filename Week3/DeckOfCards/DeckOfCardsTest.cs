// DeckOfCardsTest.cs
// Card shuffling and dealing application.

public class DeckOfCardsTest
{
    // execute application
    public static void Main(string[] args)
    {
        TestShuffle();
        TestSameFacesInHand();
    }

    public static void TestShuffle()
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
        Console.WriteLine();
    }

    public static void TestSameFacesInHand()
    {
        DeckOfCards myDeckOfCards = new DeckOfCards();
        myDeckOfCards.Shuffle();

        myDeckOfCards.FillHand();
        myDeckOfCards.PrintHand();
        Console.WriteLine(myDeckOfCards.TwoSameFacesInHand());
    }
}

