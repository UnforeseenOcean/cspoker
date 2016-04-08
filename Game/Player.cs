namespace Game
{
    class Player
    {
        public Card[] pocket;
        public int Stack { get; set; }
        public Hand Hand { get; private set; }
        public bool Fold { get; set; }
        public bool AllIn { get; set; } // 
        public int StartingStack { get; set; } //
        public int LastBet { get; set; }
        public int ID { get; }
        private static int id = 0;
        
        public Player(int Stack)
        {
            pocket = new Card[2];
            this.Stack = Stack;
            Fold = false;
            AllIn = false;
            LastBet = 0;
            ID = id++;
        }

        public void Evaluate(Card[] tableCards)
        {
            HandEvaluator he = new HandEvaluator();
            Hand = he.Evaluate(pocket, tableCards);
        }
    }
}
