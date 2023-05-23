using Meadow.Peripherals.Sensors;
using System;

namespace AvaloniaSample.Services
{
    internal class SensorService
    {
        public ITemperatureSensor _tempSensor;
        public IHumiditySensor _humiditySensor;

        public SensorService(ITemperatureSensor temperatureSensor, IHumiditySensor humiditySensor)
        {
            _tempSensor = temperatureSensor;
            _humiditySensor = humiditySensor;

            _tempSensor.StartUpdating(TimeSpan.FromSeconds(1));
            _humiditySensor.StartUpdating(TimeSpan.FromSeconds(1));
        }
    }
}
