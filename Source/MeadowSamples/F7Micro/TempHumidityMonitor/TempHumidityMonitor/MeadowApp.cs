using System;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Displays.Tft;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Sensors.Atmospheric;

namespace TempHumidityMonitor
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        GraphicsLibrary display;
        SHT31D sensor;

        public MeadowApp()
        {
            InitHardware();

            Monitor();
        }

        void InitHardware()
        {
            Console.WriteLine("Initialize hardware...");

            var st7789 = new ST7789(Device, Device.CreateSpiBus(6000),
                Device.Pins.D02, Device.Pins.D01, Device.Pins.D00,
                135, 240);

            display = new GraphicsLibrary(st7789);
            display.CurrentFont = new Font12x20();
            display.CurrentRotation = GraphicsLibrary.Rotation._270Degrees;

            sensor = new SHT31D(Device.CreateI2cBus());
        }

        void Monitor()
        {
            while(true)
            {
                display.Clear();
                display.DrawText(0, 0, $"Temp: {sensor.Temperature}", Meadow.Foundation.Color.Yellow);
                display.DrawText(0, 30, $"Humidity: {sensor.Humidity}", Meadow.Foundation.Color.LightBlue);
                display.Show();
            }
        }
    }
}