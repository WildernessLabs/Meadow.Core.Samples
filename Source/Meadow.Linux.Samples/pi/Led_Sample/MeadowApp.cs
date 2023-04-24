using Meadow;
using Meadow.Pinouts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DigitalIOSample
{
    public class MeadowApp : App<Linux<RaspberryPi>>
    {
        public static async Task Main(string[] args)
        {
            await MeadowOS.Start(args);
        }

        public override Task Run()
        {
            // this is pin 40 on a Pi4, so last outer pin (easy to clip with a scope)            
            var output = Device.CreateDigitalOutputPort(Device.Pins.Pin40);

            // the above uses the pin number.  If you prefer using the logical GPIO number, the equivalent would be
            //var output = Device.CreateDigitalOutputPort(Device.Pins.GPIO21);

            var state = false;

            while (true)
            {
                output.State = state;
                Console.WriteLine($"{(state ? "ON" : "OFF")}");
                state = !state;
                Thread.Sleep(1000);
            }
        }
    }

    /*
     * SAMPLE WIRING
     * 
          5V |
          5V |
    Pi4  GND |----------(LED)-------------
         ... |                            |
          40 |-----[resistor]-------------
    */
}
