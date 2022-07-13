using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using Meadow.Units;
using System;
using System.Threading.Tasks;

namespace AnalogInputPort
{
    public class MeadowApp : App<F7FeatherV2>
    {
        IAnalogInputPort analogIn;

        public override Task Initialize()
        {
            Console.WriteLine("Initializing hardware...");

            analogIn = Device.CreateAnalogInputPort(Device.Pins.A00);

            analogIn.Updated += (s, result) =>
            {
                Console.WriteLine($"Analog event, new voltage: {result.New.Volts:N2}V, old: {result.Old?.Volts:N2}V");
            };

            var observer = IAnalogInputPort.CreateObserver(
                handler: result =>
                {
                    Console.WriteLine($"Analog observer triggered; new: {result.New.Volts:n2}V, old: {result.Old?.Volts:n2}V");
                },
                // filter is optional. in this case, we're only notifying if the
                // voltage changes by at least `0.1V`.
                filter: result =>
                {
                    if (result.Old is { } oldValue)
                    {
                        return (result.New - oldValue).Abs().Volts > 0.1;
                    }
                    else { return false; }
                }
            );
            analogIn.Subscribe(observer);

            Console.WriteLine("Hardware initialized.");

            return base.Initialize();
        }

        public override async Task Run()
        {
            Voltage voltageReading = await analogIn.Read();
            Console.WriteLine($"Voltages: {voltageReading.Volts:N3}");

            analogIn.StartUpdating(TimeSpan.FromSeconds(1));
        }
    }
}