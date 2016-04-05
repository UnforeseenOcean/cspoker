namespace Game
{
    using System;

    class Deck
    {
        private Card[] Cards;
        private bool[] cardIsAvailable;
        private int remainingCards;

        public Deck()
        {

            remainingCards = 52;

            cardIsAvailable = new bool[52];

            for (int i = 0; i < 52; i++)
            {
                cardIsAvailable[i] = true;
            }

            string[] names = new string[13] { "2", "3", "4", "5", "6", "7",
                "8", "9", "10", "J", "Q", "K", "A" };

            string[] suits = new string[4] { "s", "h", "c", "d" };

            Cards = new Card[52];

            int c = 0;
            for (int i = 0; i < 13; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Cards[c] = new Card(names[i], suits[j], i + 2);
                    ++c;
                }
            }
        }

        public Card Draw()
        {
            if (remainingCards == 0)
            {
                Console.WriteLine("ERROR: No cards left in the deck");
            }

            Random rnd = new Random();
            int n = rnd.Next(0, 52);
            while (cardIsAvailable[n] == false)
            {
                n = rnd.Next(0, 52);
            }

            --remainingCards;
            cardIsAvailable[n] = false;
            return Cards[n];
        }

        public void Reset()
        {
            for (int i = 0; i < 52; i++)
            {
                cardIsAvailable[i] = true;
            }
            remainingCards = 52;
        }
    }
}
