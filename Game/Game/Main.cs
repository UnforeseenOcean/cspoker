﻿namespace Game
{
    using System;

    class Program
    {
        static void Main(string[] args)
        {
            Table table = new Table(2, 1, 2);
            //table.DrawCards();
            //table.ShowHands();
            //table.test();

            table.playHand();
            Console.ReadKey();
        }
    }
}
