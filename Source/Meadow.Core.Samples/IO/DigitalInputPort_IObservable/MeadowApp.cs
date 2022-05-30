using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using System;

namespace DigitalInputPort_IObservable_Sample
{
    /// <summary>
    /// This sample illustrates using the IFilterableObserver pattern. To wire up, add
    /// a PushButton connected to D02, with the circuit terminating on the 3.3V rail, so that
    /// when the button is pressed, the input is raised high.
    /// </summary>
    public class MeadowApp : App<F7FeatherV2, MeadowApp>
    {
        IDigitalInputPort input;

        public MeadowApp()
        {
            Initialize();
        }

        void Initialize()
        {
            Console.WriteLine("Initializing hardware...");

            //==== create an input port on D02. 
            input = Device.CreateDigitalInputPort(
                Device.Pins.D02, InterruptMode.EdgeBoth, ResistorMode.InternalPullDown,
                20, 10);

            //==== Classic .NET Events
            input.Changed += (object sender, DigitalPortResult result) => {
                Console.WriteLine($"Old school event raised; Time: {result.New.Time}, Value: {result.New.State}");
            };

            //===== Filterable Observer
            // this illustrates using a FilterableObserver. Note that the filter is an optional
            // parameter, if you're interested in all notifications, don't pass a filter/predicate.
            // in this case, we filter on events by time, and only notify if the new event is > 1 second from
            // the last event.
            var observer = IDigitalInputPort.CreateObserver(
                handler: result => {
                    Console.WriteLine($"Observer filter satisfied, time: {result.New.Time.ToShortTimeString()}");
                },
                // Optional filter paramter, showing a 1 second filter, i.e., only notify
                // if the new event is > 1 second from last time it was notified.
                filter: result => {
                    if (result.Old is { } old) { // C# 8 null pattern matching for not null
                        return (result.New.Time - old.Time) > TimeSpan.FromSeconds(1);
                    } else return false;
                }
                // OR, for all events, use:
                // filter: null
                );
            input.Subscribe(observer);

            Console.WriteLine("Hardware initialized.");
        }
    }
}
