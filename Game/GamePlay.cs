namespace Game
{
    using System;

    class Gameplay
    {
        private Table table;

        public Gameplay(Table table)
        {
            this.table = table;
        }

        private void ShowTable(int numToShow, string roundName)
        {
            Console.Write("\n-------------------------\n" + roundName + ": ");
            for (int i = 0; i < numToShow; i++)
                Console.Write(table.tableCards[i].Rank + table.tableCards[i].Suit + " ");

            Console.Write("\n-------------------------\n\n");
        }

        private void processBets()
        {
            foreach (Player p in table.Players)
            {
                p.Stack -= p.LastBet;
                table.Pot += p.LastBet;
                p.LastBet = 0;
            }
        }

        private void payBlinds() // not finished. only use when finished. it is broken.
        {
            if (table.Players[0].Stack >= table.smallBlind)
            {
                table.Players[0].Stack -= table.smallBlind;
                table.Pot += table.smallBlind;
            }
            else
                table.Players[0].Fold = true;

            if (table.Players[1].Stack >= table.bigBlind)
            {
                table.Players[1].Stack -= table.bigBlind;
                table.Pot += table.bigBlind;
            }
            else
                table.Players[1].Fold = true;
        }

        public void start()
        {
            int tempPot = 0;
            //int totalPot = smallBlind + bigBlind;
            int totalPot = 0;
            int betNumber = 0;
            //int highestBet = bigBlind;
            int highestBet = 0;
            bool oneLeft = false;

            HandComparer hc = new HandComparer();
            Player[] winners = null;

            foreach (Player p in table.Players) // this will help with the AllIn calculation. will it help, though? dunno. I'll check tomorrow.
                p.StartingStack = p.Stack;

            table.Reset();
            table.DrawCards();

            //payBlinds();
            while (betNumber < 4)
            {
                int minRaise = 0;
                bool raise = false;
                bool finishedRaises = false;

            RAISE:
                foreach (Player p in table.Players)
                {
                    // -----------------------------------------
                    int folded = 0;
                    foreach (Player e in table.Players)
                        if (e.Fold == true)
                            ++folded;

                    if (folded == table.Players.Length - 1)
                    {
                        processBets();
                        oneLeft = true;
                        goto ONELEFT;
                    }
                    // -----------------------------------------

                    if (raise == true)
                    {
                        if (p.LastBet == highestBet)
                        {
                            finishedRaises = true;
                            break;
                        }
                    }

                    Console.WriteLine("\nPOT: " + (totalPot + tempPot));

                    if (p.Fold == true) continue;

                    // -----------------------------------------
                    Console.Write("P" + p.ID + ": ");
                    foreach (Card c in p.pocket)
                        Console.Write(c.Rank + c.Suit + " ");

                    Console.Write("\nSTACK: " + p.Stack);

                    string input = null;
                    while (input != "check" &&
                        input != "call" &&
                        input != "raise" &&
                        input != "fold")
                    {
                        Console.Write("\ncheck/call/raise/fold: ");
                        input = Console.ReadLine();

                        if (input == "check" && highestBet != p.LastBet)
                            input = null;
                    }

                    if (input == "fold")
                    {
                        p.Fold = true;
                        continue;
                    }

                    int bet = 0;
                    if (input == "call")
                    {
                        bet = highestBet;
                        Console.WriteLine("Call " + bet);
                    }

                    if (input == "raise")
                    {
                        while (Int32.TryParse(input, out bet) == false ||
                        bet > p.Stack ||
                        bet < (highestBet + minRaise))
                        {
                            Console.Write("Raise to: ");
                            input = Console.ReadLine();
                        }
                    }

                    if (bet > highestBet)
                    {
                        raise = true;
                        minRaise = bet - highestBet;
                    }

                    if (bet > p.Stack) bet = p.Stack;

                    highestBet = bet;

                    p.LastBet = bet;

                    int tPot = 0;
                    foreach (Player e in table.Players)
                        tPot += e.LastBet;

                    tempPot = tPot;
                }

                if (raise == true && finishedRaises == false) goto RAISE;

                processBets();

                if (betNumber == 0)
                    ShowTable(3, "FLOP");
                else if (betNumber == 1)
                    ShowTable(4, "TURN");
                else if (betNumber == 2)
                    ShowTable(5, "RIVER");

                ++betNumber;
                totalPot += tempPot;
                tempPot = 0;
                minRaise = 0;
                highestBet = 0;
            }

        ONELEFT:

            hc.players = table.Players;
            hc.tableCards = table.tableCards;
            winners = hc.Evaluate();

            string winsOrTies = "wins";
            if (winners.Length > 1)
                winsOrTies = "ties";

            int prize = table.Pot / winners.Length;
            foreach (Player p in winners)
            {
                p.Stack += prize;
                Console.Write("\nPlayer " + p.ID + " " + winsOrTies + " main pot: " + prize + " chips.\n");
                if (oneLeft == false)
                {
                    Console.Write("With hand: " + p.Hand.Name.ToString().Replace("_", " ") + " (");

                    foreach (Card c in p.Hand.Cards)
                        Console.Write(c.Rank + c.Suit + " ");

                    Console.Write(")\n");
                }
            }
        }
    }
}
