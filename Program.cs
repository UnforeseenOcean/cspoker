using System;

namespace Game
{

    class ASD
    {
        public int a;
    }

    class Program
    {
        static void Main(string[] args)
        {
            //Table table = new Table(9, 1, 2);
            //table.DrawCards();
            //table.ShowHands();

            ASD asd = new ASD();
            asd.a = 100;

            ASD bsd = asd;
            bsd = asd;

            asd.a = 200;
            Console.WriteLine(asd.a);
            Console.WriteLine(bsd.a);

            Console.ReadKey();
        }
    }
} // namespace
