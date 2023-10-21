// DeckOfCards.cs
// DeckOfCards class represents a deck of playing cards.

public class DeckOfCards
{
    #region Private Fields
    private Card[] deck; // array of Card objects
    private int currentCard; // index of next Card to be dealt
    private const int NUMBER_OF_CARDS = 52; // constant number of Cards
    private const int CARDS_IN_HAND = 5;
    private Random randomNumbers; // random number generator
    private Card[] handOfCards; // current hand of 5 cards 
    private string[] faces = { "Ace", "Deuce", "Three", "Four", "Five", "Six",
         "Seven", "Eight", "Nine", "Ten", "Jack", "Queen", "King" };
    private string[] suits = { "Hearts", "Diamonds", "Clubs", "Spades" };
    #endregion

    #region Constructors
    // constructor fills deck of Cards
    public DeckOfCards()
    {


        deck = new Card[NUMBER_OF_CARDS]; // create array of Card objects
        handOfCards = new Card[CARDS_IN_HAND]; // create array of Card objects representing the hand
        currentCard = 0; // set currentCard so deck[ 0 ] is dealt first  
        randomNumbers = new Random(); // create random number generator

        // populate deck with Card objects
        for (int count = 0; count < deck.Length; count++)
            deck[count] =
               new Card(faces[count % 13], suits[count / 13]);
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Recommended method used to store how many of each of the cards
    /// we have in our hand. (i.e. a hand "Ace Spades", "Jack Hearts", "Jack Spades", "Two Diamonds", "Ace Diamonds" will
    /// look like [1,0,0,0,0,0,0,0,0,2,0,0,2])
    /// </summary>
    /// <returns></returns>
    private int[] totalHand()
    {
        // Initialize the count array with zeroes 
        int[] countPerFace = new int[13];
        Array.Fill(countPerFace, 0);

        // Count the occurence of each face 
        foreach (Card card in handOfCards)
        {
            int cardIndex = Array.IndexOf(faces, card.Face);
            countPerFace[cardIndex]++;
        }

        return countPerFace;
    }
    #endregion

    #region Public Methods
    // shuffle deck of Cards with one-pass algorithm
    public void Shuffle()
    {
        // after shuffling, dealing should start at deck[ 0 ] again
        currentCard = 0; // reinitialize currentCard

        // for each Card, pick another random Card and swap them
        for (int first = 0; first < deck.Length; first++)
        {
            // select a random number between 0 and 51 
            int second = randomNumbers.Next(NUMBER_OF_CARDS);

            // swap current Card with randomly selected Card
            Card temp = deck[first];
            deck[first] = deck[second];
            deck[second] = temp;
        }
    }

    // deal one Card
    public Card? DealCard()
    {
        // determine whether Cards remain to be dealt
        if (currentCard < deck.Length)
            return deck[currentCard++]; // return current Card in array
        else
            return null; // indicate that all Cards were dealt
    }

    /// <summary>
    /// Fills the current hand with cards by drawing the cards from the deck
    /// If the deck has no more cards, a message gets displayed which prompts 
    /// the user to reshuffle the deck and start again 
    /// </summary>
    public void FillHand()
    {
        // Check if we have enough cards, if not the ask to reshuffle the deck
        if (currentCard >= deck.Length - 5)
        {
            Console.WriteLine("Not enough cards to fill the hand anew.");
            return;
        }

        // Draw 5 cards to fill the hand
        for (int i = 0; i < CARDS_IN_HAND; i++)
            handOfCards[i] = DealCard();
    }

    /// <summary>
    /// Draw 5 cards to fill the current hand then check if there are 2 cards
    /// whoose faces match
    /// </summary>
    /// <param name="face">Optional parameter to check for that specifeid face. Check for any otherwise</param>
    /// <returns>True if there are two cards of the same face, false otherwise</returns>
    public bool TwoSameFacesInHand()
    {
        if (handOfCards.Length == 0)
        {
            Console.WriteLine("Cannot check on an empty hand.");
            return false;
        }

        int[] countPerFace = totalHand();
        return countPerFace.Where(x => x == 2).Count() >= 1 ? true : false;
    }

    /// <summary>
    /// Check if the current hand  has 2 cards
    /// whoose faces match and another 2 cards whoose faces match
    /// </summary>
    /// <param name="face"></param>
    /// <returns></returns>
    public bool TwoTimesTwoSameFacesInHand()
    {
        if (handOfCards.Length == 0)
        {
            Console.WriteLine("Cannot check on an empty hand.");
            return false;
        }

        int[] countPerFace = totalHand();
        return countPerFace.Where(x => x == 2).Count() == 2 ? true : false;
    }

    /// <summary>
    /// Check if the current hand has 3 cards whose faces match
    /// </summary>
    /// <returns>True if the hand contains 3 cards of the same face, false otherwise</returns>
    public bool ThriceSameFaceInHand()
    {
        if (handOfCards.Length == 0)
        {
            Console.WriteLine("Cannot check on an empty hand.");
            return false;
        }

        int[] countPerFace = totalHand();
        return countPerFace.Where(x => x == 3).Count() == 1 ? true : false;
    }

    /// <summary>
    /// Check if the current hand has 4 cards of the same face
    /// </summary>
    /// <returns>True if the hand contains 4 cards of the same hand, false otherwise</returns>
    public bool FourSameFacesInHand()
    {
        if (handOfCards.Length == 0)
        {
            Console.WriteLine("Cannot check on an empty hand.");
            return false;
        }

        int[] countPerFace = totalHand();
        return countPerFace.Where(x => x == 4).Count() == 1 ? true : false;
    }

    /// <summary>
    /// Check if there are 5 cards with increasingly going faces (i.e. Two, Three, Four, Five, Six)
    /// </summary>
    /// <returns>True if there are 5 cards with increasingly going faces, false otherwise</returns>
    public bool FiveCardsInARow()
    {
        if (handOfCards.Length == 0)
        {
            Console.WriteLine("Cannot check on an empty hand.");
            return false;
        }

        int[] countPerFace = totalHand();
        int counter = 0;
        foreach (int times in countPerFace)
        {
            if (times > 0)
                counter++;
            else
                counter = 0;
        }

        return counter == 5 ? true : false;
    }

    public void PrintHand()
    {
        for (int i = 0; i < handOfCards.Length; i++)
        {
            Console.Write("{0,-19}", handOfCards[i]);
        }
    }
    #endregion
}
