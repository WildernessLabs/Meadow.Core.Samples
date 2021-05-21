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
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        IDigitalInputPort input;

        public MeadowApp()
        {
            // create an input port on D02. 
            input = Device.CreateDigitalInputPort(
                Device.Pins.D02, InterruptMode.EdgeBoth, ResistorMode.InternalPullDown,
                20, 10);

            //==== Classic .NET Events
            input.Changed += (object sender, DigitalInputPortChangeResult result) => {
                Console.WriteLine($"Old school event raised; Time: {result.New.Millisecond}, Value: {result.Value}");
            };

            ////===== Filterable Observer
            //// this illustrates using a FilterableObserver. Note that the filter is an optional
            //// parameter, if you're interested in all notifications, don't pass a filter/predicate.
            //// in this case, we filter on events by time, and only notify if the new event is > 1 second from
            //// the last event. 
            //_input.Subscribe(new FilterableChangeObserver<DigitalInputPortChangeResult>(
            //    handler: result => {
            //        Console.WriteLine($"Observer Observing the Observable, Observably speaking, Time: {result.New.New.Millisecond}");
            //    },
            //    // Optional filter paramter, showing a 1 second filter, i.e., only notify
            //    // if the new event is > 1 second from last.
            //    filter: result => {
            //        if (result.Old is { } old) {
            //            return (result.New.)
            //        }
            //        return (f.Delta > new TimeSpan(0, 0, 0, 0, 1000));
            //    }));

            Console.WriteLine("Got here 3.");

        }
    }
}
