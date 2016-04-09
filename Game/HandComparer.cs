namespace Game
{
    class HandComparer
    {
        public Player[] players;
        private Player[] tiedPlayers;
        public Card[] tableCards;
        private int bestHandValue;
        private int ties;
        private int[] playerScore;
        private int highestScore;

        public HandComparer(Player[] Players, Card[] TableCards)
        {
            this.players = Players;
            this.tableCards = TableCards;
            highestScore = 0;
            bestHandValue = 0;
            ties = 0;
        }

        private void findBestHands()
        {
            for (int i = 0; i < players.Length; i++)
                if ((int)players[i].Hand.Name > bestHandValue && players[i].Fold == false)
                    bestHandValue = (int)players[i].Hand.Name;

            for (int i = 0; i < players.Length; i++)
                if ((int)players[i].Hand.Name == bestHandValue && players[i].Fold == false)
                    ++ties;
        }

        private Player[] gatherWinners()
        {
            foreach (int e in playerScore)
            {
                if (e > highestScore)
                    highestScore = e;
            }

            int winnerCount = 0;
            foreach (int e in playerScore)
            {
                if (e == highestScore)
                {
                    ++winnerCount;
                }
            }

            Player[] winners = new Player[winnerCount];
            int c = 0;
            for (int i = 0; i < tiedPlayers.Length; i++)
            {
                if (playerScore[i] == highestScore)
                    winners[c++] = tiedPlayers[i];
            }

            return winners;
        }

        private Player[] untie()
        {
            for (int i = 0; i < ties; i++)
            {
                for (int j = 0; j < ties; j++)
                {
                    for (int k = 0; k < 5; k++)
                    {
                        if (i != j)
                        {
                            if (tiedPlayers[i].Hand.Cards[k].Value > tiedPlayers[j].Hand.Cards[k].Value)
                            {
                                ++playerScore[i];
                                playerScore[j] -= 100;
                                break;
                            }
                            else if (tiedPlayers[i].Hand.Cards[k].Value < tiedPlayers[j].Hand.Cards[k].Value)
                            {
                                ++playerScore[j];
                                playerScore[i] -= 100;
                                break;
                            }
                        }
                    }
                }
            }           
            return gatherWinners();
        }

        private Player[] findWinners()
        {
            tiedPlayers = new Player[ties];
            playerScore = new int[tiedPlayers.Length];

            int c = 0;
            for (int i = 0; i < players.Length; i++)
                if ((int)players[i].Hand.Name == bestHandValue && players[i].Fold == false)
                    tiedPlayers[c++] = players[i];

            if (ties == 1)
                return tiedPlayers;
            else
                return untie();
        }

        public Player[] Evaluate()
        {
            bestHandValue = 0;
            ties = 0;
            highestScore = 0;

            for (int i = 0; i < players.Length; i++)
                players[i].Evaluate(tableCards);

            findBestHands();
            return findWinners();
        }
    }
}
