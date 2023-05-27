using Meadow;
using Meadow.Pinouts;
using System;
using System.Threading.Tasks;

namespace PushButton_Sample
{
    public class MeadowApp : App<Linux<RaspberryPi>>
    {
        public static async Task Main(string[] args)
        {
            await MeadowOS.Start(args);
        }

        public override async Task Run()
        {

            Console.WriteLine($"Platform:    {Device.Information.Platform}");
            Console.WriteLine($"Device Name: {Device.Information.DeviceName}");
            Console.WriteLine($"Model:       {Device.Information.Model}");
            Console.WriteLine($"OS version:  {Device.PlatformOS.OSVersion}");

            while (true)
            {
                Console.Write($"CPU Temp: {Device.PlatformOS.GetCpuTemperature().Celsius:N2}C    \r");
                await Task.Delay(1000);
            }
        }

    }
}
