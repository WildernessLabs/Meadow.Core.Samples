using Meadow;
using Meadow.Devices;
using System;
using System.Threading;

namespace MeadowApp
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        public MeadowApp()
        {
            while (true) {

                Console.WriteLine($"Battery Voltage: {Device.GetBatteryLevel().Volts:N2}V");

                Thread.Sleep(3000);
            }
        }
    }
}