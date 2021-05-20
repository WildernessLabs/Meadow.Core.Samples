using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using Meadow.Units;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Basic_AnalogReads
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        IAnalogInputPort analogIn00;

        public MeadowApp()
        {
            Console.WriteLine("Starting App");

            // configure our analog port
            Initialize();

            //==== Do a one-off read
            PerformOneRead().Wait();

            //==== Start updating
            analogIn00.StartUpdating();
        }

        void Initialize()
        {
            Console.WriteLine("Initializing hardware...");

            //==== create our analog input port
            analogIn00 = Device.CreateAnalogInputPort(Device.Pins.A00);

            //==== Classic .NET Events
            analogIn00.Updated += (s, result) => {
                Console.WriteLine($"Analog event, new voltage: {result.New.Volts:N2}V, old: {result.Old?.Volts:N2}V");
            };

            //==== Filterable Observable
            var observer = IAnalogInputPort.CreateObserver(
                handler: result => {
                    Console.WriteLine($"Analog observer triggered; new: {result.New.Volts:n2}V, old: {result.Old?.Volts:n2}V");
                },
                // filter is optional. in this case, we're only notifying if the
                // voltage changes by at least `0.1V`.
                filter: result => {
                    if (result.Old is { } oldValue) {
                        return (result.New - oldValue).Abs().Volts > 0.1;
                    } else { return false; }
                }
            );
            analogIn00.Subscribe(observer);

            Console.WriteLine("Hardware initialized.");
        }

        protected async Task PerformOneRead()
        {
            // Analog port returns a `Voltage` unit
            Voltage voltageReading = await analogIn00.Read();
            Console.WriteLine($"Voltages: {voltageReading.Volts:N3}");
        }
    }
}
