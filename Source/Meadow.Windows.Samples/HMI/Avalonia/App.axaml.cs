using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AvaloniaMeadow.ViewModels;
using AvaloniaMeadow.Views;
using Meadow;
using Meadow.Hardware;
using Meadow.UI;
using System.Threading.Tasks;

namespace AvaloniaMeadow
{
    public partial class App : AvaloniaMeadowApplication<Windows>
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

        public override Task MeadowInitialize()
        {
            var ft = new Meadow.Foundation.ICs.IOExpanders.Ft232h();
            var led = ft.Pins.C0.CreateDigitalOutputPort(false);
            Resolver.Services.Add(led);

            return Task.CompletedTask;
        }
    }
}