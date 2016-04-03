using System;

namespace Game
{
    class Program
    {
        static void Main(string[] args)
        {
            Table table = new Table(9, 1, 2);
            table.DrawCards();
            table.ShowHands();
            table.test();
            Console.ReadKey();
        }
    }
} // namespace
