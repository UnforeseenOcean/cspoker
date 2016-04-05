namespace Game
{
    class Card
    {
        public string Rank { get; }
        public string Suit { get; }
        public int Value { get; }

        public Card(string Rank, string Suit, int Value)
        {
            this.Rank = Rank;
            this.Suit = Suit;
            this.Value = Value;
        }
    }
}
