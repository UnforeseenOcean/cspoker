﻿namespace Game
{
    class Player
    {
        public Card[] pocket;
        public int Stack { get; set; }
        public Hand hand { get; private set; }
        public bool Fold { get; }
        public int ID { get; }
        private static int id = 0;

        public Player(int Stack)
        {
            pocket = new Card[2];
            this.Stack = Stack;
            Fold = false;
            ID = id++;
        }

        public void Evaluate(Card[] tableCards)
        {
            HandEvaluator he = new HandEvaluator();
            hand = he.Evaluate(pocket, tableCards);
        }
    }
}
