using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using System;
using System.Threading;

namespace Basic_AnalogReads
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        IAnalogInputPort _a00;
        IAnalogInputPort _a01;

        public MeadowApp()
        {
            Console.WriteLine("Starting App");
            _a00 = Device.CreateAnalogInputPort(Device.Pins.A00);
            _a01 = Device.CreateAnalogInputPort(Device.Pins.A01);
            Console.WriteLine("Analog port created");
            this.StartReading();
        }

        protected async void StartReading()
        {
            float v0, v1;

            while(true) {
                v0 = await _a00.Read(1);
                Thread.Sleep(1000);
                v1 = await _a01.Read(1);
                Console.WriteLine($"Voltages: {v0}\t{v1}");
                Thread.Sleep(1000);
            }
        }
    }
}
