using System;

namespace Game
{
    class Card {
        public string Name { get; }
        public string Suit { get; }
        public int Value { get; }

        public Card(string Name, string Suit, int Value) {
            this.Name = Name;
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

            string[] names = new string[13] { "A", "2", "3", "4", "5", "6", "7",
                "8", "9", "10", "J", "Q", "K" };

            string[] suits = new string[4] { "s", "h", "c", "d" };

            Cards = new Card[52];

            int c = 0;
            for (int i = 0; i < 13; i++) {
                for (int j = 0; j < 4; j++) {
                    Cards[c] = new Card(names[i], suits[j], i);
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
        public Card[] hole;
        public int Stack { get; set; }

        public Player(int Stack) {
            hole = new Card[2];
            this.Stack = Stack;
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
                p.hole[0] = deck.Draw();
                p.hole[1] = deck.Draw();
            }
        }

        public void ShowHands() {
            foreach (Player p in Players) {
                Console.Write(p.hole[0].Name + p.hole[0].Suit + " ");
                Console.WriteLine(p.hole[1].Name + p.hole[1].Suit + " ");
                Console.WriteLine("------------------");
            }
        }

        public void Reset() {
            deck.Reset();
            Pot = 0;
        }
    }
} // namespace
