namespace Game
{
    using System;

    class HandEvaluator
    {
        private Card[] allCards;
        private Card[] bestCards;

        public HandEvaluator()
        {
            allCards = new Card[7];
            bestCards = new Card[5];
        }

        private void sort(Card[] cards) // proffesional sort algorithmz
        {
            Card hold = null;
            for (int i = 0; i < cards.Length - 1; i++)
            {
                for (int j = 0; j < (cards.Length - i) - 1; j++)
                {
                    if (cards[j].Value > cards[j + 1].Value)
                    {
                        hold = cards[j];
                        cards[j] = cards[j + 1];
                        cards[j + 1] = hold;
                    }
                }
            }
        }

        private void addKickers(int numKickers)
        {
            int bestSlot = 5 - numKickers;
            int ignoredCards = bestSlot;
            bool notAvailable = false;
            for (int i = allCards.Length - 1; i >= 0; i--)
            {
                for (int j = 0; j < ignoredCards; j++) if (bestCards[j].Value == allCards[i].Value) notAvailable = true;
                if (bestSlot < 5 && notAvailable == false) bestCards[bestSlot++] = allCards[i];
                notAvailable = false;
            }
        }

        private Card[] removeDuplicate() // proffesional sort algorithmz²
        {

            int numDup = 0;
            int[] dupPos = new int[5] { -1, -1, -1, -1, -1 };
            bool skip = false;
            for (int i = 0; i < allCards.Length - 1; i++)
            {
                for (int j = i + 1; j < allCards.Length - 1; j++)
                {
                    for (int k = 0; k < dupPos.Length - 1; k++)
                        if (j == dupPos[k])
                            skip = true;

                    if (allCards[i].Value == allCards[j].Value && skip == false)
                        dupPos[numDup++] = j;

                    skip = false;
                }

            }

            Card[] dupFree = new Card[allCards.Length - numDup];
            int numStored = 0;
            skip = false;
            for (int i = 0; i < allCards.Length; i++)
            {
                foreach (int e in dupPos)
                    if (e == i) skip = true;

                if (skip == false)
                    dupFree[numStored++] = allCards[i];

                skip = false;
            }
            return dupFree;
        }

        private Hand highCard()
        {
            Array.Copy(allCards, 2, bestCards, 0, 5);
            return new Hand(HandName.High_Card, bestCards);
        }

        private Hand pairs(int ignoreValue = 0)
        {
            int bestSlot = 0;
            for (int i = allCards.Length - 1; i >= 1; i--)
            {
                for (int j = i - 1; j >= 0; j--)
                {
                    if (allCards[i].Value == allCards[j].Value &&
                        (allCards[i].Value != ignoreValue && allCards[j].Value != ignoreValue))
                    {
                        if (bestSlot == 4) goto Finish;
                        bestCards[bestSlot++] = allCards[i];
                        bestCards[bestSlot++] = allCards[j];
                    }
                }
            }

        Finish:

            if (bestSlot == 0) return null;
            else if (bestSlot == 2)
            {
                addKickers(3);
                return new Hand(HandName.Pair, bestCards);
            }
            else {
                addKickers(1);
                return new Hand(HandName.Two_Pairs, bestCards);
            }
        }

        private Hand threeOfAKind()
        {
            int bestSlot = 0;
            for (int i = allCards.Length - 1; i >= 1; i--)
            {
                for (int j = i - 1; j >= 0; j--)
                {
                    if (allCards[i].Value == allCards[j].Value)
                    {
                        for (int k = j - 1; k >= 0; k--)
                        {
                            if (allCards[i].Value == allCards[k].Value)
                            {
                                bestCards[bestSlot++] = allCards[i];
                                bestCards[bestSlot++] = allCards[j];
                                bestCards[bestSlot++] = allCards[k];
                                addKickers(2);
                                return new Hand(HandName.Three_of_a_Kind, bestCards);
                            }
                        }
                    }
                }
            }
            return null;
        }

        private Hand straight(bool straightFlushTest = false)
        {
            Card[] dupFree = null;
            if (straightFlushTest == true)
                dupFree = allCards;
            else
                dupFree = removeDuplicate();


            if (dupFree.Length < 5) return null;

            // ACE low straight: A, 2, 3, 4, 5
            if (dupFree[dupFree.Length - 1].Value == 14 &&
                dupFree[0].Value == 2 &&
                dupFree[1].Value == 3 &&
                dupFree[2].Value == 4 &&
                dupFree[3].Value == 5)
            {
                Array.Copy(dupFree, 0, bestCards, 1, 4);
                bestCards[0] = dupFree[dupFree.Length - 1];
                return new Hand(HandName.Straight, bestCards);
            }

            // Regular straights
            int highestSequence = 0;
            for (int i = dupFree.Length - 1; i >= 4; i--)
            {
                for (int j = i; j >= i - 4; j--)
                {
                    if (dupFree[j - 1].Value == dupFree[j].Value - 1)
                    {
                        ++highestSequence;
                        if (highestSequence == 4)
                        {
                            Array.Copy(dupFree, j - 1, bestCards, 0, 5);
                            return new Hand(HandName.Straight, bestCards);
                        }
                    }
                    else {
                        highestSequence = 0;
                        break;
                    }
                }
            }
            return null;
        }

        private Hand flush()
        {
            int highestSequence = 0;
            for (int i = allCards.Length - 1; i >= 4; i--)
            {
                bestCards[highestSequence++] = allCards[i];
                for (int j = i - 1; j >= 0; j--)
                {
                    if (allCards[i].Suit == allCards[j].Suit)
                    {
                        bestCards[highestSequence++] = allCards[j];
                        if (highestSequence == 5)
                            return new Hand(HandName.Flush, bestCards);
                    }
                }
                highestSequence = 0;
            }
            return null;
        }

        private Hand fullHouse()
        {
            Hand toak = threeOfAKind();
            if (toak == null) return null;

            Card[] fhCards = new Card[5];
            fhCards[0] = toak.Cards[0];
            fhCards[1] = toak.Cards[1];
            fhCards[2] = toak.Cards[2];

            Hand pair = pairs(toak.Cards[0].Value);
            if (pair == null) return null;

            fhCards[3] = pair.Cards[0];
            fhCards[4] = pair.Cards[1];

            return new Hand(HandName.Full_House, fhCards);
        }

        private Hand fourOfAKind() // what matters is that it works
        {
            int bestSlot = 0;
            for (int i = allCards.Length - 1; i >= 1; i--)
            {
                for (int j = i - 1; j >= 0; j--)
                {
                    if (allCards[i].Value == allCards[j].Value)
                    {
                        for (int k = j - 1; k >= 0; k--)
                        {
                            if (allCards[i].Value == allCards[k].Value)
                            {
                                for (int l = k - 1; l >= 0; l--)
                                {
                                    if (allCards[k].Value == allCards[l].Value)
                                    {
                                        bestCards[bestSlot++] = allCards[i];
                                        bestCards[bestSlot++] = allCards[j];
                                        bestCards[bestSlot++] = allCards[k];
                                        bestCards[bestSlot++] = allCards[l];
                                        addKickers(1);
                                        return new Hand(HandName.Four_of_a_Kind, bestCards);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }

        private Hand straightFlush() // HAHAHA this one sucks
        {
            Card[] backUp = new Card[7];
            Array.Copy(allCards, 0, backUp, 0, 7);

            int highestSequence = 0;
            Card[] sfCards = new Card[7];
            for (int i = 0; i < allCards.Length; i++)
            {
                sfCards[highestSequence++] = allCards[i];
                for (int j = i + 1; j < allCards.Length; j++)
                {
                    if (allCards[i].Suit == allCards[j].Suit)
                        sfCards[highestSequence++] = allCards[j];
                }
                if (highestSequence >= 5)
                {
                    Card[] sfCardsFit = new Card[highestSequence];
                    Array.Copy(sfCards, 0, sfCardsFit, 0, highestSequence);
                    allCards = sfCardsFit;
                    Hand hand = straight(straightFlushTest: true);
                    if (hand != null)
                    {
                        return new Hand(HandName.Straight_Flush, bestCards);
                    }
                    else {
                        allCards = backUp;
                        return null;
                    }
                }
                highestSequence = 0;
            }
            allCards = backUp;
            return null;
        }

        public Hand Evaluate(Card[] pocket, Card[] tableCards)
        {
            pocket.CopyTo(allCards, 0);
            tableCards.CopyTo(allCards, 2);
            sort(allCards);
            Hand hand = null;
            if ((hand = straightFlush()) != null) return hand;
            if ((hand = fourOfAKind()) != null) return hand;
            if ((hand = fullHouse()) != null) return hand;
            if ((hand = flush()) != null) return hand;
            if ((hand = straight()) != null) return hand;
            if ((hand = threeOfAKind()) != null) return hand;
            if ((hand = pairs()) != null) return hand;
            return highCard();
        }
    }
}
