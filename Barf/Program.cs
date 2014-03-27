using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barf
{
    class Program
    {
        static void Main(string[] args)
        {
            // This is a joke.
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            {
                Console.WriteLine("Compressing example.txt");
                Barf.Compress("example.txt");

                Console.WriteLine("Extracting example.barf");
                Barf.Extract("example.barf");
            }
            stopwatch.Stop();
            Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);
        }
    }
}
