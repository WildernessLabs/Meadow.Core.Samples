using Meadow;
using Meadow.Peripherals.Sensors;
using Meadow.Units;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AvaloniaSample.Simulation
{
    public class SimulatedTempSensor : SimulatedSensor<Temperature>, ITemperatureSensor
    {
        public Temperature? Temperature => LastReading;

        public event EventHandler<IChangeResult<Temperature>> TemperatureUpdated = delegate { };

        protected override void RaiseUpdatedEvent(Temperature newValue)
        {
            TemperatureUpdated?.Invoke(this, new ChangeResult<Temperature>(newValue, LastReading));
        }

        protected override Task<Temperature> ReadSensor()
        {
            lock (base.samplingLock)
            {
                var last = LastReading ?? new Temperature(65, Meadow.Units.Temperature.UnitType.Fahrenheit);
                var delta = Random.Shared.NextSingle();
                if (Random.Shared.NextSingle() > 0.5f) delta *= -1;
                var newTemp = new Temperature(last.Fahrenheit + delta, Meadow.Units.Temperature.UnitType.Fahrenheit);
                RaiseUpdatedEvent(newTemp);
                Debug.WriteLine($"New temperature: {newTemp.Fahrenheit:N1}F");
                return Task.FromResult(newTemp);
            }
        }
    }
}