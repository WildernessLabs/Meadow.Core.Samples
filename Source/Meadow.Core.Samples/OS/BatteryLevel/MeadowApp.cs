using System;
using System.Threading;
using Meadow;
using Meadow.Devices;

namespace MeadowApp
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {

        public MeadowApp()
        {
            Initialize();

            while (true) {

                Console.WriteLine($"Battery Voltage: {Device.GetBatteryLevel()}V");

                Thread.Sleep(3000);
            }
        }

        void Initialize()
        {
            Console.WriteLine("Initialize hardware...");
            Device.InitCoProcessor();

            Console.WriteLine("Initialization complete.");
        }
    }
}