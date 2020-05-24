using TempHumidityMonitor;
using Meadow;
using System.Threading;

namespace TempHumidityMonitor
{
    class Program
    {
        static IApp app;
        public static void Main(string[] args)
        {
            // instantiate and run new meadow app
            app = new MeadowApp();

            Thread.Sleep(Timeout.Infinite);
        }
    }
}