namespace Game
{
    class Player
    {
        public Card[] pocket;
        public int Stack { get; set; }
        public Hand hand { get; private set; }
        public bool Fold { get; set; }
        public int ID { get; }
        private static int id = 0;
        public int LastBet { get; set; }

        public Player(int Stack)
        {
            pocket = new Card[2];
            this.Stack = Stack;
            Fold = false;
            ID = id++;
            LastBet = 0;
        }

        public void Evaluate(Card[] tableCards)
        {
            HandEvaluator he = new HandEvaluator();
            hand = he.Evaluate(pocket, tableCards);
        }
    }
}
