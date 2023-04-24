using Meadow;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Hardware;
using Meadow.Pinouts;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PushButton_Sample
{
    public class MeadowApp : App<Linux<RaspberryPi>>
    {
        private List<PushButton> _pushButtons;

        public static async Task Main(string[] args)
        {
            await MeadowOS.Start(args);
        }

        public override Task Initialize()
        {
            Console.WriteLine("Initializing...");

            _pushButtons = new List<PushButton>();

            // DEV NOTE:
            // this sample uses *external* resistors because internal resistor is only supported on OSes that have GPIOD support
            // 32-bit Raspberry Pi OS still uses the older sysfs driver as of the writing of this sample, even though the hardware supports resistors

            Console.WriteLine("Creating button on pin 40...");
            var inputExternalPullUp = Device.CreateDigitalInputPort(
                pin: Device.Pins.GPIO21, // same as Device.Pins.Pin40
                InterruptMode.EdgeBoth,
                resistorMode: ResistorMode.ExternalPullUp);
            var buttonExternalPullUp = new PushButton(inputExternalPullUp);

            _pushButtons.Add(buttonExternalPullUp);

            Console.WriteLine("Creating button on pin 38...");
            var inputExternalPullDown = Device.CreateDigitalInputPort(
                pin: Device.Pins.GPIO20, // same as Device.Pins.Pin38
                InterruptMode.EdgeBoth,
                resistorMode: ResistorMode.ExternalPullDown);
            var buttonExternalPullDown = new PushButton(inputExternalPullDown);

            _pushButtons.Add(buttonExternalPullDown);

            Console.WriteLine("Wiring up event handlers...");
            foreach (var pushButton in _pushButtons)
            {
                pushButton.LongClickedThreshold = new TimeSpan(0, 0, 1);

                pushButton.Clicked += PushButtonClicked;
                pushButton.PressStarted += PushButtonPressStarted;
                pushButton.PressEnded += PushButtonPressEnded;
                pushButton.LongClicked += PushButtonLongClicked;
            }

            return Task.CompletedTask;
        }

        void PushButtonClicked(object sender, EventArgs e)
        {
            Console.WriteLine($"PushButton Clicked!");
            Thread.Sleep(500); // this provides a simple "debounce"
        }

        void PushButtonPressStarted(object sender, EventArgs e)
        {
            Console.WriteLine($"PushButton PressStarted!");
        }

        void PushButtonPressEnded(object sender, EventArgs e)
        {
            Console.WriteLine($"PushButton PressEnded!");
        }

        void PushButtonLongClicked(object sender, EventArgs e)
        {
            Console.WriteLine($"PushButton LongClicked!");
            Thread.Sleep(500); // this provides a simple "debounce"
        }
    }
}
