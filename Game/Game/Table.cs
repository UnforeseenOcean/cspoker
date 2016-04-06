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

        public void ShowTable(int numToShow, string roundName)
        {
            Console.Write("-------------------------\n" + roundName + ": ");
            for (int i = 0; i < numToShow; i++)
                Console.Write(tableCards[i].Rank + tableCards[i].Suit + " ");

            Console.Write("\n-------------------------\n\n");
        }

        private void processBets()
        {
            foreach (Player p in Players)
            {
                p.Stack -= p.LastBet;
                Pot += p.LastBet;
                p.LastBet = 0;
            }
        }

        public void playHand() // this function is just a test. it will be gone soon
        {
            int tempPot = 0;
            int betNumber = 0;
            bool oneLeft = false;

            HandComparer hc = new HandComparer();
            Player[] winners = null;

            Reset();
            DrawCards();

            while (betNumber < 4)
            {
                int highestBet = 0;
                bool raise = false;
                bool finishedRaises = false;
                
                RAISE:
                foreach (Player p in Players)
                {
                    Console.WriteLine("\nPOT: " + tempPot);
                    if (raise == true)
                    {
                        if (p.LastBet == highestBet)
                        {
                            finishedRaises = true;
                            break;
                        }
                    }

                    // -----------------------------------------
                    int folded = 0;
                    foreach (Player e in Players)
                        if (e.Fold == true)
                            ++folded;

                    if (folded == Players.Length - 1)
                    {
                        processBets();
                        oneLeft = true;
                        goto ONELEFT;
                    }
                    // -----------------------------------------

                    if (p.Fold == true) continue;

                    // -----------------------------------------
                    Console.Write("P" + p.ID + ": ");
                    foreach (Card c in p.pocket)
                        Console.Write(c.Rank + c.Suit + " ");

                    Console.Write("\nSTACK: " + p.Stack);

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

                        while (Int32.TryParse(input, out bet) == false || bet < highestBet ||
                            bet > p.Stack)
                        {
                            Console.Write("Raise to: ");
                            input = Console.ReadLine();
                        }

                        if (bet > highestBet) raise = true;
                        
                        if (bet > p.Stack) bet = p.Stack;

                        highestBet = bet;
                        Console.WriteLine("\nHIGHEST BET: " + highestBet);

                        p.LastBet = bet;

                        int tPot = 0;
                        foreach (Player e in Players)
                            tPot += e.LastBet;

                        tempPot = tPot;
                    }
                    Console.Write("\n");
                    // -----------------------------------------
                }

                if (raise == true && finishedRaises == false) goto RAISE;
                
                processBets();

                // -----------------------------------------
                if (betNumber == 0)
                    ShowTable(3, "FLOP");
                else if (betNumber == 1)
                    ShowTable(4, "TURN");
                else if (betNumber == 2)
                    ShowTable(5, "RIVER");

                ++betNumber;
                // -----------------------------------------

            }

        ONELEFT:

            hc.players = Players;
            hc.tableCards = tableCards;
            winners = hc.Evaluate();

            string winsOrTies = "wins";
            if (winners.Length > 1)
                winsOrTies = "ties";

            int prize = Pot / winners.Length;
            foreach (Player p in winners)
            {
                p.Stack += prize;
                Console.Write("\nPlayer " + p.ID + " " + winsOrTies + " main pot: " + prize + " chips.\n");
                if (oneLeft == false)
                {
                    Console.Write("With hand: " + p.hand.Name.ToString().Replace("_", " ") + " (");

                    foreach (Card c in p.hand.Cards)
                        Console.Write(c.Rank + c.Suit + " ");

                    Console.Write(")\n");
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
                        Console.Write("\n\nWITH HAND: " + p.hand.Name.ToString().Replace("_", " "));
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
            } while (winners[0].hand.Name != HandName.Straight_Flush || winners.Length < 2);
        }

        public void Reset()
        {
            deck.Reset();
            Pot = 0;
        }
    }
}