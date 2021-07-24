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
            app.Run();

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
