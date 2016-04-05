namespace Game
{
    class Hand
    {
        public HandName handName { get; }
        public Card[] Cards { get; }

        public Hand(HandName handName, Card[] Cards)
        {
            this.handName = handName;
            this.Cards = Cards;
        }
    }
}
