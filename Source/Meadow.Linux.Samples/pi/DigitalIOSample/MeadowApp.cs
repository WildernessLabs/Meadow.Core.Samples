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
            var state = false;

            while (true)
            {
                Console.Write(".");
                output.State = state;
                state = !state;
                Thread.Sleep(1000);
            }
        }

        void RunLoopback()
        {
            /*
            // this is pin 40 on a Pi4, so last outer pin (easy to clip with a scope)
            var output = Device.CreateDigitalOutputPort(Device.Pins.GPIO21);
            var state = false;

            var input = Device.CreateDigitalInputPort(Device.Pins.GPIO20, Meadow.Hardware.InterruptMode.EdgeRising, Meadow.Hardware.ResistorMode.InternalPullUp);
            input.Changed += (s, e) =>
            {
                Console.WriteLine($"Interrupt: {e.New.State}");
            };

            while (true)
            {
                output.State = state;
                Console.WriteLine($"{(state ? 'H' : 'L')} -> {(input.State ? 'H' : 'L')}");

                state = !state;
                Thread.Sleep(1000);
            }
            */
        }
    }
}
