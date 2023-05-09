using System;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AvaloniaMeadow.ViewModels;
using AvaloniaMeadow.Views;
using AvaloniaSample.Services;
using AvaloniaSample.Simulation;
using Meadow;
using Meadow.Foundation.Sensors.Atmospheric;
using Meadow.Peripherals.Sensors;
using Meadow.Pinouts;
using System.Threading.Tasks;
using Meadow.Foundation.Leds;
using Meadow.Hardware;

namespace AvaloniaMeadow
{
    public partial class App : AvaloniaMeadowApplication<Linux<RaspberryPi>>
    {
        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    Content = new MainView()
                    {
                        DataContext = new MainWindowViewModel(),
                    }
                };
            }
            else if(ApplicationLifetime is ISingleViewApplicationLifetime singleView)
            {
                singleView.MainView = new MainView
                {
                    DataContext = new MainWindowViewModel(),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);

            LoadMeadowOS();
        }

        public override Task InitializeMeadow()
        {
            var r = Resolver.Services.Get<IMeadowDevice>();

            var led = Device.Pins.Pin40.CreateDigitalOutputPort(false);
            Resolver.Services.Add<IDigitalOutputPort>(led);
            
            if (r == null)
            {
                Resolver.Log.Info("IMeadowDevice is null");
            }
            else
            {
                Resolver.Log.Info($"IMeadowDevice is {r.GetType().Name}");
            }

            var useSimulation = true;

            if (useSimulation)
            {
                Resolver.Services.Create<SimulatedTempSensor, ITemperatureSensor>();
                Resolver.Services.Create<SimulatedHumiditySensor, IHumiditySensor>();
            }
            else
            {
                var bus = Device.CreateI2cBus(1);
                var htu = new Htu21d(bus);

                // the HTU21 provides both temp and humidity, so just register it twice
                Resolver.Services.Add<ITemperatureSensor>(htu);
                Resolver.Services.Add<IHumiditySensor>(htu);
            }

            Resolver.Services.Create<SensorService>();

            return Task.CompletedTask;
        }
    }
}