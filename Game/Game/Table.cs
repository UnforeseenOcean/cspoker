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

        public void ShowTable(int numToShow)
        {
            for (int i = 0; i < numToShow - 1; i++)
                Console.Write(tableCards[i].Rank + tableCards[i].Suit + " ");
        }

        public void StartRounds_test() // this function is just a test. it will be gone soon
        {
            int roundNumber = 0;

            HandComparer hc = new HandComparer();
            Player[] winners = null;

            Reset();
            DrawCards();

            while (roundNumber < 4)
            {
                foreach (Player p in Players)
                {
                    if (p.Fold == true) continue;

                    Console.Write("P" + p.ID + ": ");
                    foreach (Card c in p.pocket)
                        Console.Write(c.Rank + c.Suit + " ");

                    string input = null;
                    while (input != "f" && input != "b")
                    {
                        Console.Write("\nFold or Bet: ");
                        input = Console.ReadLine();
                    }

                        
                    if (input == "f")
                        p.Fold = true;
                    else
                    {
                        int bet;
                        Console.Write("Type your bet: ");
                        while (Int32.TryParse(input, out bet) == false)
                            input = Console.ReadLine();

                        if (bet > p.Stack) bet = p.Stack;

                        p.Stack -= bet;
                        Pot += bet;
                    }
                    Console.Write("\n");
                }

                if (roundNumber == 0)
                {
                    Console.Write("FLOP: ");
                    ShowTable(3);
                } else if (roundNumber == 1)
                {
                    Console.Write("TURN: ");
                    ShowTable(4);
                } else if (roundNumber == 2)
                {
                    Console.Write("RIVER: ");
                    ShowTable(5);
                }

                ++roundNumber;
            }

            hc.players = Players;
            hc.tableCards = tableCards;
            winners = hc.Evaluate();

            if (winners.Length == 1)
            {
                winners[0].Stack += Pot;
                Console.Write("\nPlayer " + winners[0].ID + " wins main pot for " + Pot + "\n");
            } 
            else
            {
                int prize = Pot / winners.Length;
                foreach (Player p in winners)
                {
                    p.Stack += prize;
                    Console.Write("\nPlayer " + winners[0].ID + " ties main pot for " + prize + "\n");
                }
            }
        }

        public void test() // this function is just a test. it will be gone soon
        {
            HandComparer hc = new HandComparer();
            Player[] winners = null;
            do
            {
            AGAIN:
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
            } while (winners[0].hand.handName != HandName.two_pairs || winners.Length < 2);
        }

        public void Reset()
        {
            deck.Reset();
            Pot = 0;
        }
    }
}