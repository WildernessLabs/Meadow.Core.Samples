using Meadow;
using Meadow.Hardware;
using ReactiveUI;
using System.Reactive;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AvaloniaMeadow.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private IDigitalOutputPort? _led;
        private string _buttonText;

        public ReactiveCommand<Unit, Unit> LedCommand { get; }

        public MainWindowViewModel()
        {
            ButtonText = "Initializing...";
            LedCommand = ReactiveCommand.Create(ToggleLed);

            // since Avalonia and Meadow are both starting at the same time, we must wait
            // for MeadowInitialize to complete before the output port is ready
            _ = Task.Run(WaitForLed);
        }

        [ObservableProperty]
        private string buttonText;

        private async Task WaitForLed()
        {
            while (_led == null)
            {
                _led = Resolver.Services.Get<IDigitalOutputPort>();
                await Task.Delay(100);
            }
            ButtonText = "Turn LED On";
        }

        private void ToggleLed()
        {
            if (_led == null) return;

            _led.State = !_led.State;
            if (_led.State)
            {
                ButtonText = "Turn LED Off";
            }
            else
            {
                ButtonText = "Turn LED On";
            }
        }
    }
}