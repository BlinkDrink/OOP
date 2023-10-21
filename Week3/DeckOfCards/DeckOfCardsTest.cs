// DeckOfCardsTest.cs
// Card shuffling and dealing application.

public class DeckOfCardsTest
{
    // execute application
    public static void Main(string[] args)
    {
        DeckOfCards deckOfCards = new DeckOfCards();
        Console.WriteLine("------------------------------------");
        Console.WriteLine("Testing Shuffle()");
        TestShuffle(deckOfCards);
        Console.WriteLine("------------------------------------");
        Console.WriteLine("Testing TwoSameFacesInHand()");
        TestSameFacesInHand(deckOfCards);
        Console.WriteLine("------------------------------------");
        Console.WriteLine("Testing TwoTimesTwoSameFacesInHand()");
        TestTwoTimesTwoSameFacesInHand(deckOfCards);
        Console.WriteLine("------------------------------------");
        Console.WriteLine("Testing ThriceSameFaceInHand()");
        TestThriceSameFaceInHand(deckOfCards);
        Console.WriteLine("------------------------------------");
        Console.WriteLine("Testing FourSameFacesInHand()");
        TestFourSameFacesInHand(deckOfCards);
    }

    public static void TestShuffle(DeckOfCards deckOfCards)
    {
        deckOfCards.Shuffle(); // place Cards in random order

        // display all 52 Cards in the order in which they are dealt
        for (int i = 0; i < 52; i++)
        {
            Console.Write("{0,-19}", deckOfCards.DealCard());

            if ((i + 1) % 4 == 0)
                Console.WriteLine();
        }
        Console.WriteLine();
    }

    public static void TestSameFacesInHand(DeckOfCards deckOfCards)
    {
        deckOfCards.Shuffle();
        while (true)
        {
            deckOfCards.FillHand();
            deckOfCards.PrintHand();
            Console.WriteLine(deckOfCards.TwoSameFacesInHand());
            Console.WriteLine("Do you want to refill your hand and try again (y/n)?");
            string answer = Console.ReadLine();
            if (answer == "n")
                break;
        }
    }

    public static void TestTwoTimesTwoSameFacesInHand(DeckOfCards deckOfCards)
    {
        while (true)
        {
            deckOfCards.Shuffle();
            deckOfCards.FillHand();
            deckOfCards.PrintHand();
            Console.WriteLine(deckOfCards.TwoTimesTwoSameFacesInHand());
            Console.WriteLine("Do you want to refill your hand and try again (y/n)?");
            string answer = Console.ReadLine();
            if (answer == "n")
                break;
        }
    }

    public static void TestThriceSameFaceInHand(DeckOfCards deckOfCards)
    {
        while (true)
        {
            deckOfCards.Shuffle();
            deckOfCards.FillHand();
            deckOfCards.PrintHand();
            Console.WriteLine(deckOfCards.ThriceSameFaceInHand());
            Console.WriteLine("Do you want to refill your hand and try again (y/n)?");
            string answer = Console.ReadLine();
            if (answer == "n")
                break;
        }
    }

    public static void TestFourSameFacesInHand(DeckOfCards deckOfCards)
    {
        while (true)
        {
            deckOfCards.Shuffle();
            deckOfCards.FillHand();
            deckOfCards.PrintHand();
            Console.WriteLine(deckOfCards.FourSameFacesInHand());
            Console.WriteLine("Do you want to refill your hand and try again (y/n)?");
            string answer = Console.ReadLine();
            if (answer == "n")
                break;
        }
    }
}

