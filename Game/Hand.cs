namespace Game
{
    class Hand
    {
        public HandName Name { get; }
        public Card[] Cards { get; }

        public Hand(HandName Name, Card[] Cards)
        {
            this.Name = Name;
            this.Cards = Cards;
        }
    }
}
