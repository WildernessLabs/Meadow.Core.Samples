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
    public class MeadowApp : App<F7FeatherV2, MeadowApp>
    {
        private Logger _logger;
        private SPIDisplay _spiDisplay;

        public MeadowApp()
        {
            Initialize();

            _ = Task.Run(() => SpiDisplayProc());
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
            
            _logger.Info($"Creating SPI display...");
            var spi = Device.CreateSpiBus(St7789.DefaultSpiBusSpeed);

            _spiDisplay = new SPIDisplay(
                Device, 
                spi, 
                Device.Pins.D17, // cs
                Device.Pins.D18, // dc
                Device.Pins.D19, // res
                _logger);

        }
    }
}
