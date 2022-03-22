using System;
using System.Threading;
using System.Threading.Tasks;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Displays.TftSpi;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Light;
using Meadow.Hardware;
using Meadow.Logging;

namespace MeadowApp
{
    public class CcmPinTests
    {
        private IMeadowDevice _device;
        private Logger _logger;

        public CcmPinTests(IMeadowDevice device, Logger logger)
        {
            _device = device;
            _logger = logger;
        }

        public async Task TestSPI5ControlPins(int iterations)
        {
            // this method is intended to allow testing the display control pins with a scope
            var chipSelect = _device.CreateDigitalOutputPort(_device.GetPin("D17"));
            var dc = _device.CreateDigitalOutputPort(_device.GetPin("D18"));
            var reset = _device.CreateDigitalOutputPort(_device.GetPin("D19"));

            await Task.Run(async () =>
            {
                while (iterations > 0)
                {
                    var state = false;

                    for (int i = 0; i < 20; i++)
                    {
                        _logger.Info($"CS {state}");
                        chipSelect.State = state;
                        await Task.Delay(500);
                    }

                    for (int i = 0; i < 20; i++)
                    {
                        _logger.Info($"DC {state}");
                        chipSelect.State = state;
                        await Task.Delay(500);
                    }

                    for (int i = 0; i < 20; i++)
                    {
                        _logger.Info($"RES {state}");
                        chipSelect.State = state;
                        await Task.Delay(500);
                    }

                    iterations--;
                }
            });
        }
    }

    public class MeadowApp : App<F7CoreCompute, MeadowApp>
    {
        private Logger _logger;
        private SPIDisplay _spiDisplay;
        private I2CDisplay _i2cDisplay;
        private IDigitalOutputPort _led;

        public MeadowApp()
        {
            Initialize();

            _ = Task.Run(() => BlinkyProc());
            _ = Task.Run(() => SpiDisplayProc());
        }

        private void BlinkyProc()
        {
            var state = false;

            _i2cDisplay.ShowText("LED", 0);
           
            while (true)
            {
                _logger.Info(state ? "ON" : "OFF");
                _i2cDisplay.ShowText(state ? "ON" : "OFF", 1);

                _led.State = state;
                Thread.Sleep(5000);
                state = !state;
            }
        }

        private void SpiDisplayProc()
        {
            while (true)
            {
                var now = DateTime.Now.ToString("HH:mm:ss"); ;
                _logger.Info($"now: {now}");
                _spiDisplay.ShowText(now, 1);

                Thread.Sleep(1000);
            }
        }


        void Initialize()
        {
            _logger = new Logger(new ConsoleLogProvider());

            _logger.Info("Initialize hardware...");

            var i2cdisplaybus = 3;

            _logger.Info($"Creating I2C display on bus {i2cdisplaybus}...");
            _i2cDisplay = new I2CDisplay(
                Device.CreateI2cBus(i2cdisplaybus), 
                _logger);
            
            var spidisplaybus = 5;

            _logger.Info($"Creating SPI display on bus {spidisplaybus}...");
            var spi = Device.CreateSpiBus(St7789.DefaultSpiBusSpeed, spidisplaybus);

            _spiDisplay = new SPIDisplay(
                Device, 
                spi, 
                Device.Pins.D17, // cs
                Device.Pins.D18, // dc
                Device.Pins.D19, // res
                _logger);

            _led = Device.CreateDigitalOutputPort(Device.Pins.D20);
            
        }
    }
}
