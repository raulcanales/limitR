using System;
using System.Threading;
using System.Threading.Tasks;

namespace limitR.Samples.ConsoleApp
{
    class Program
    {
        private static readonly IRateLimiter rl = new SimpleRateLimiter(2, TimeSpan.FromSeconds(5));

        static void Main(string[] args)
        {
            Print(rl.Invoke(() => { return DateTime.Now; }).ToString());
            Thread.Sleep(500);
            for (var i = 0; i < 60; i++)
            {
                Print(rl.Invoke(() =>
                {
                    return DateTime.Now;
                }).ToString());
            }

            Console.ReadLine();
        }

        static void Print(string text)
        {
            Console.WriteLine(text);
        }
    }
}
