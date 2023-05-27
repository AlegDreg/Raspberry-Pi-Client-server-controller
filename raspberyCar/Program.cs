using System;

namespace raspberyCar
{
    internal class Program
    {
        private static Hub Hub;
        static void Main(string[] args)
        {
            Hub = new Hub("http://192.168.0.181:5000/bot");

            Console.WriteLine("Запущено");

            while (true)
                Console.ReadKey();
        }
    }
}