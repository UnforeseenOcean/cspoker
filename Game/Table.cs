namespace Game
{
    using System;

    class Table
    {
        private Deck deck;
        public Player[] Players;
        public Card[] tableCards;
        public int bigBlind { get; }
        public int smallBlind { get; }
        public int Pot { get; set; }

        public Table(int numPlayers, int bigBlind, int smallBlind)
        {
            deck = new Deck();
            Players = new Player[numPlayers];
            tableCards = new Card[5];
            Pot = 0;

            for (int i = 0; i < numPlayers; i++)
            {
                Players[i] = new Player(1500);
            }

            this.bigBlind = bigBlind;
            this.smallBlind = smallBlind;
        }

        public void DrawCards()
        {
            foreach (Player p in Players)
            {
                p.pocket[0] = deck.Draw();
                p.pocket[1] = deck.Draw();
            }

            for (int i = 0; i < 5; i++)
                tableCards[i] = deck.Draw();
        }

        public void ShowHands()
        {
            foreach (Player p in Players)
            {
                Console.Write(p.pocket[0].Rank + p.pocket[0].Suit + " ");
                Console.WriteLine(p.pocket[1].Rank + p.pocket[1].Suit + " ");
                Console.WriteLine("------------------");
            }

            foreach (Card c in tableCards)
            {
                Console.Write(c.Rank + c.Suit + " ");
            }
        }

        public void startGame()
        {
            while(true)
            {
                Gameplay round = new Gameplay(this);
                round.start();
            }
            
        }

        public void Reset()
        {
            deck.Reset();
            Pot = 0;
        }

        // finds a specific hand for testing.
        public void test() 
        {
            HandComparer hc = new HandComparer(Players, tableCards);
            Player[] winners = null;
            do
            {
            AGAIN:
                Reset();
                DrawCards();
                Console.Write("\n\n");
                ShowHands();

                winners = hc.Evaluate();

                if (winners != null)
                {
                    foreach (Player p in winners)
                    {
                        Console.Write("\n\nWINNER IS: " + p.ID);
                        Console.Write("\n\nWITH HAND: " + p.Hand.Name.ToString().Replace("_", " "));
                        Console.Write("\n\nCARDS: ");
                        foreach (Card card in p.Hand.Cards)
                        {
                            Console.Write(card.Rank + card.Suit + " ");
                        }
                    }
                }
                else {
                    Console.Write("\n\nTHERE IS A TIE");
                    goto AGAIN;
                }
            } while (winners[0].Hand.Name != HandName.Flush);
        }
    }
}