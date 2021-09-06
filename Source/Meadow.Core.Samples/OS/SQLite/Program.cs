using System;
using System.Threading;
using Meadow;

namespace MeadowApp
{
    class Program
    {
        public static void Main(string[] args)
        {
            // instantiate and run new meadow app
            var app = new MeadowApp();

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
