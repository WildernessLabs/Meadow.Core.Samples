using Avalonia;
using Avalonia.Threading;
using Meadow;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AvaloniaMeadow
{
    /*
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

        public override Task InitializeMeadow()
        {
            var d = Device.PlatformOS.OSVersion;

            var ft = new Meadow.Foundation.ICs.IOExpanders.Ft232h();
            var led = ft.Pins.C0.CreateDigitalOutputPort(false);
            Resolver.Services.Add(led);

            return Task.CompletedTask;
        }
    }
    */

    public class AvaloniaMeadowApplication<T> : Application, IApp
        where T : class, IMeadowDevice
    {
        public CancellationToken CancellationToken => throw new NotImplementedException();

        public static T Device => Resolver.Services.Get<IMeadowDevice>() as T;

        protected AvaloniaMeadowApplication()
        {
        }

        public void InvokeOnMainThread(Action<object?> action, object? state = null)
        {
            Dispatcher.UIThread.Post(() => action(state));
        }

        virtual public Task OnError(Exception e)
        {
            return Task.CompletedTask;
        }

        virtual public Task OnShutdown()
        {
            return Task.CompletedTask;
        }

        virtual public void OnUpdate(Version newVersion, out bool approveUpdate)
        {
            approveUpdate = true;
        }

        virtual public void OnUpdateComplete(Version oldVersion, out bool rollbackUpdate)
        {
            rollbackUpdate = false;
        }

        virtual public Task Run()
        {
            return Task.CompletedTask;
        }

        public virtual Task InitializeMeadow()
        {
            return Task.CompletedTask;
        }

        Task IApp.Initialize()
        {
            return InitializeMeadow();
        }

        protected void LoadMeadowOS()
        {
            new Thread((o) =>
            {
                _ = MeadowOS.Start(this, null);
            })
            {
                IsBackground = true
            }
            .Start();
        }
    }
}