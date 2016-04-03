using System;

namespace Game
{
    class Card {
        public string Rank { get; }
        public string Suit { get; }
        public int Value { get; }

        public Card(string Rank, string Suit, int Value) {
            this.Rank = Rank;
            this.Suit = Suit;
            this.Value = Value;
        }
    }

    class Deck {
        private Card[] Cards;
        private bool[] cardIsAvailable;
        private int remainingCards;

        public Deck() {

            remainingCards = 52;

            cardIsAvailable = new bool[52];

            for (int i = 0; i < 52; i++) {
                cardIsAvailable[i] = true;
            }

            string[] names = new string[13] { "2", "3", "4", "5", "6", "7",
                "8", "9", "10", "J", "Q", "K", "A" };

            string[] suits = new string[4] { "s", "h", "c", "d" };

            Cards = new Card[52];

            int c = 0;
            for (int i = 0; i < 13; i++) {
                for (int j = 0; j < 4; j++) {
                    Cards[c] = new Card(names[i], suits[j], i+2);
                    ++c;
                }
            }
        }
        
        public Card Draw()
        {
            if (remainingCards == 0) {
                Console.WriteLine("ERROR: No cards left in the deck");
            }

            Random rnd = new Random();
            int n = rnd.Next(0, 52);
            while (cardIsAvailable[n] == false) {
                n = rnd.Next(0, 52);
            }

            --remainingCards;
            cardIsAvailable[n] = false;
            return Cards[n];
        }

        public void Reset() {
            for (int i = 0; i < 52; i++) {
                cardIsAvailable[i] = true;
            }
            remainingCards = 52;
        }
    }

    class Player {
        public Card[] pocket;
        public int Stack { get; set; }
        public Hand hand { get; private set; }

        public Player(int Stack) {
            pocket = new Card[2];
            this.Stack = Stack;
        }

        public void Evaluate(Card[] tableCards)
        {
            HandEvaluator he = new HandEvaluator();
            hand = he.Evaluate(pocket, tableCards);
        }
    }

    class Table {
        private Deck deck;
        public Player[] Players;
        public Card[] tableCards;
        private int bigBlind;
        private int smallBlind;
        public int Pot { get; set; }

        public Table(int numPlayers, int bigBlind, int smallBlind) {
            deck = new Deck();
            Players = new Player[numPlayers];
            tableCards = new Card[5];
            Pot = 0;

            for (int i = 0; i < numPlayers; i++) {
                Players[i] = new Player(1500);
            }

            this.bigBlind = bigBlind;
            this.smallBlind = smallBlind;
        }

        public void DrawCards() {
            foreach (Player p in Players) {
                p.pocket[0] = deck.Draw();
                p.pocket[1] = deck.Draw();
            }
            tableCards[0] = deck.Draw();
            tableCards[1] = deck.Draw();
            tableCards[2] = deck.Draw();
            tableCards[3] = deck.Draw();
            tableCards[4] = deck.Draw();
        }

        public void ShowHands() {
            foreach (Player p in Players) {
                Console.Write(p.pocket[0].Rank + p.pocket[0].Suit + " ");
                Console.WriteLine(p.pocket[1].Rank + p.pocket[1].Suit + " ");
                Console.WriteLine("------------------");
            }

            foreach (Card c in tableCards) {
                Console.Write(c.Rank + c.Suit + " ");
            }
        }

        public void test()
        {
            int counter = 0;
            Players[0].Evaluate(tableCards);
            Console.WriteLine("\nPlayer 0 best hand: ");
            foreach(Card c in Players[0].hand.Cards)
            {
                Console.Write(c.Rank + c.Suit + " ");
            }
            Console.WriteLine(Players[0].hand.handName.ToString());
            while (Players[0].hand.handName != HANDNAME.three_of_a_kind)
            {
                Reset();
                DrawCards();
                //ShowHands();
                Players[0].Evaluate(tableCards);
                /*Console.WriteLine("\nPlayer 0 best hand: ");
                foreach (Card c in Players[0].hand.Cards)
                {
                    Console.Write(c.Rank + c.Suit + " ");
                }
                Console.WriteLine(Players[0].hand.handName.ToString());
                ++counter;
                Console.WriteLine("COUNTER: " + counter);*/
                ++counter;
                //Console.WriteLine("COUNTER: " + counter);
            }
            ShowHands();
            Console.WriteLine("\nPlayer 0 best hand: ");
            foreach (Card c in Players[0].hand.Cards)
            {
                Console.Write(c.Rank + c.Suit + " ");
            }
            Console.WriteLine(Players[0].hand.handName.ToString());
            ++counter;
            Console.WriteLine("COUNTER: " + counter);
        }

        public void Reset() {
            deck.Reset();
            Pot = 0;
        }
    }
} // namespace
