using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using System;

namespace AnalogObserver
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        IAnalogInputPort _analogIn;

        protected IDisposable _absoluteObserver = null;

        public MeadowApp()
        {
            Console.WriteLine("Starting App");
            this.InitializeIO();
            this.WireUpObservers();
        }

        public void InitializeIO()
        {
            _analogIn = Device.CreateAnalogInputPort(Device.Pins.A00);
            Console.WriteLine("Analog port created");
        }

        public void WireUpObservers()
        {
            var firehoseSubscriber = _analogIn.Subscribe(new FilterableChangeObserver<FloatChangeResult, float>(
                handler: result =>
                {
                    Console.WriteLine("Previous Value: " + result.Old);
                    Console.WriteLine("New Value: " + result.New);
                }));

            //firehoseSubscriber.Dispose();

            // absolute: notify me when the temperature hits 75º
            float seventyFiveDegreesC = (75f - 32f) * (5f / 9f); // convert to C
            float seventyFiveDegreesVoltage = (seventyFiveDegreesC / 100f) * 3.3f;
            _absoluteObserver = _analogIn.Subscribe(new FilterableChangeObserver<FloatChangeResult, float>(
                filter: result => (result.New > seventyFiveDegreesVoltage),
                handler: avgValue =>
                {
                    Console.WriteLine("We've hit 75º!");
                    // unsubscribe when we hit it
                    if (_absoluteObserver != null)
                    {
                        _absoluteObserver.Dispose();
                    }
                }));

            // relative, static comparison; e.g if change is > 1º
            float oneDegreeC = 3.3f / 100f; // TMP35DZ: 0º = 0V, 100º = 3.3V
            var relative = _analogIn.Subscribe(new FilterableChangeObserver<FloatChangeResult, float>(
                filter: result => (result.Delta > oneDegreeC || result.Delta < oneDegreeC),
                handler: result =>
                {
                    Console.WriteLine("Changed value: " + result.Delta);
                }));

            //relative.Dispose();

            // relative percentage change
            _analogIn.Subscribe(new FilterableChangeObserver<FloatChangeResult, float>(
                filter: result => (result.DeltaPercent > 10 || result.DeltaPercent < 10),
                handler: result =>
                {
                    Console.WriteLine("Percentage changed: " + result.Delta);
                }));

            // spin up the ADC sampling engine
            _analogIn.StartSampling();

        }
    }
}
