using System;
using System.Threading;
using Meadow;
using Meadow.Devices;

namespace MeadowApp
{
    /// <summary>
    /// This sample prints the MCU temp to the console.
    /// </summary>
    public class MeadowApp : App<F7FeatherV2, MeadowApp>
    {
        public MeadowApp()
        {
            while(true) {
                // get the temp
                Console.WriteLine($"Processor Temp: {Device.GetProcessorTemperature().Celsius:n2}C");
                Thread.Sleep(1000);
            }
        }
    }
}