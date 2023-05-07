using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AvaloniaMeadow.ViewModels;
using AvaloniaMeadow.Views;
using Meadow;
using Meadow.Hardware;
using Meadow.Pinouts;
using System.Threading.Tasks;

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

            if (r == null)
            {
                Resolver.Log.Info("IMeadowDevice is null");
            }
            else
            {
                Resolver.Log.Info($"IMeadowDevice is {r.GetType().Name}");
            }

            var led = Device.Pins.Pin40.CreateDigitalOutputPort(false);

            if (led == null)
            {
                Resolver.Log.Info("led is null");
            }
            else
            {
                Resolver.Log.Info($"led is {led.GetType().Name}");
                Resolver.Services.Add<IDigitalOutputPort>(led);
            }

            return Task.CompletedTask;
        }
    }
}