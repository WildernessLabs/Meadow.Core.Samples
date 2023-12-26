using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WakeOnInterrupt
{
    /// <summary>
    /// This sample illustrates putting the device into low-power (sleep) state until an interrupt occurs
    /// </summary>
    public class MeadowApp : App<F7FeatherV2>
    {
        private IDigitalOutputPort _red;
        private IDigitalOutputPort _blue;
        private IDigitalOutputPort _green;

        public TimeSpan Timespan { get; private set; }

        public override Task Initialize()
        {
            Resolver.Log.Info("Initializing hardware...");

            _red = Device.Pins.OnboardLedRed.CreateDigitalOutputPort(false);
            _green = Device.Pins.OnboardLedGreen.CreateDigitalOutputPort(false);
            _blue = Device.Pins.OnboardLedBlue.CreateDigitalOutputPort(false);

            Resolver.Services.Add<IDigitalOutputPort>(_blue);

            Device.PlatformOS.BeforeSleep += () =>
            {
                _red.State = true;
                //                Resolver.Log.Info("Sleeping...");
            };

            Device.PlatformOS.AfterWake += () =>
            {
                Thread.Sleep(1000);
                _red.State = false;
                //                _blue.State = true;
                //                Resolver.Log.Info("Resuming...");

            };

            return Task.CompletedTask;
        }

        public override async Task Run()
        {
            while (true)
            {
                //                Resolver.Log.Info("Waiting...");
                for (var i = 0; i < 5; i++)
                {
                    _green.State = true;
                    await Task.Delay(500);
                    _green.State = false;
                    await Task.Delay(500);
                }
                Device.PlatformOS.Sleep(TimeSpan.FromSeconds(5));
                //Device.PlatformOS.SleepUntilInterrupt(Device.Pins.D10, InterruptMode.EdgeFalling, ResistorMode.Disabled);

                //                Resolver.Log.Info("Continuing...");
            }
        }
    }
}