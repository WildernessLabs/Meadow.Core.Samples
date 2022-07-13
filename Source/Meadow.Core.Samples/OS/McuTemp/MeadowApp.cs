using Meadow;
using Meadow.Devices;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace McuTemp
{
    /// <summary>
    /// This sample prints the MCU temp to the console.
    /// </summary>
    public class MeadowApp : App<F7FeatherV2>
    {
        public override Task Run()
        {
            while(true) 
            {
                // get the temp
                Console.WriteLine($"Processor Temp: {Device.GetProcessorTemperature().Celsius:n2}C");
                Thread.Sleep(1000);
            }
        }
    }
}