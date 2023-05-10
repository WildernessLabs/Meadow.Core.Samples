using Meadow;
using Meadow.Peripherals.Sensors;
using Meadow.Units;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AvaloniaSample.Simulation
{
    public class SimulatedHumiditySensor : SimulatedSensor<RelativeHumidity>, IHumiditySensor
    {
        public RelativeHumidity? Humidity => LastReading;

        public event EventHandler<IChangeResult<RelativeHumidity>> HumidityUpdated = delegate { };

        protected override void RaiseUpdatedEvent(RelativeHumidity newValue)
        {
            HumidityUpdated?.Invoke(this, new ChangeResult<RelativeHumidity>(newValue, LastReading));
        }

        protected override Task<RelativeHumidity> ReadSensor()
        {
            lock (base.samplingLock)
            {
                var last = LastReading ?? new RelativeHumidity(70, RelativeHumidity.UnitType.Percent);
                var delta = Random.Shared.NextSingle() / 10f;
                if (Random.Shared.NextSingle() > 0.5f) delta *= -1;
                var newHumidity = new RelativeHumidity(last.Percent + delta, RelativeHumidity.UnitType.Percent);
                RaiseUpdatedEvent(newHumidity);
                Debug.WriteLine($"New humidity: {newHumidity.Percent:N1}%");
                return Task.FromResult(newHumidity);
            }
        }
    }
}