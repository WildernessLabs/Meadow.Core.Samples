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
                app.SQLiteNetTest();
            }
            catch(Exception ex)
            {
                Console.WriteLine($"FAILURE: {ex.Message}");
                if(ex.InnerException!= null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
