using Meadow;
using Meadow.Foundation.Sensors.Atmospheric;
using Meadow.Pinouts;
using System;
using System.Threading.Tasks;

namespace Bme280_Sample
{
    public class MeadowApp : App<Linux<RaspberryPi>>
    {
        private Bme280 _bme;

        public static async Task Main(string[] args)
        {
            await MeadowOS.Start(args);
        }

        public override Task Initialize()
        {
            Console.WriteLine("Initializing...");

            // Note: raspberry pi doesn't have a Bus 0
            var bus = Device.CreateI2cBus(1);

            _bme = new Bme280(bus);

            return Task.CompletedTask;
        }


        public override async Task Run()
        {
            while (true)
            {
                // we could also use the driver's internal sampling instead
                var data = await _bme.Read();

                Console.WriteLine($"-- Conditions --");
                Console.WriteLine($"Temp: {data.Temperature?.Fahrenheit}F");
                Console.WriteLine($"Hum:  {data.Humidity?.Percent}%");
                Console.WriteLine($"Press: {data.Pressure?.Millibar}mb");

                await (Task.Delay(1000));
            }
        }

    }
}
