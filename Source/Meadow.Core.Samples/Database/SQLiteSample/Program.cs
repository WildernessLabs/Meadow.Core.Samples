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
            app.ListFiles("/meadow0/Data");

            //            app.PInvokeTest();

            try
            {
                app.StoreData();
                Thread.Sleep(1000);
                app.StoreData();
            }
            catch(Exception ex)
            {
                Console.WriteLine($"FAILURE: {ex.Message}");
            }

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
