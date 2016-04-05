namespace Game
{
    using System;

    class Table
    {
        private Deck deck;
        public Player[] Players;
        public Card[] tableCards;
        private int bigBlind;
        private int smallBlind;
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

        public void test()
        {
            Player[] winners = null;
            do
            {
            AGAIN:
                HandComparer hc = new HandComparer();
                Reset();
                DrawCards();
                Console.Write("\n\n");
                ShowHands();
                hc.players = Players;
                hc.tableCards = tableCards;

                winners = hc.Evaluate();

                if (winners != null)
                {
                    foreach (Player p in winners)
                    {
                        Console.Write("\n\nWINNER IS: " + p.ID);
                        Console.Write("\n\nWITH HAND: " + p.hand.handName.ToString());
                        Console.Write("\n\nCARDS: ");
                        foreach (Card card in p.hand.Cards)
                        {
                            Console.Write(card.Rank + card.Suit + " ");
                        }
                    }
                }
                else {
                    Console.Write("\n\nTHERE IS A TIE");
                    goto AGAIN;
                }
            } while (winners[0].hand.handName != HandName.high_card || winners.Length < 2);
        }

        public void Reset()
        {
            deck.Reset();
            Pot = 0;
        }
    }
}