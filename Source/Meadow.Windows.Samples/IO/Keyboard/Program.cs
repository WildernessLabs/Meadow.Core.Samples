using Meadow;
using Meadow.Foundation.Sensors.Hid;
using Meadow.Hardware;
using System.Diagnostics;

public class MeadowApp : App<Windows>
{
    private Keyboard _keyBoard;
    private IDigitalInputPort _a;

    public static async Task Main(string[] _)
    {
        await MeadowOS.Start();
    }

    public override Task Initialize()
    {
        _keyBoard = new Keyboard(1);
        _a = _keyBoard.CreateDigitalInputPort(_keyBoard.Pins.A, InterruptMode.EdgeBoth);
        _a.Changed += OnKeyChanged;

        return Task.CompletedTask;
    }

    private void OnKeyChanged(object? sender, DigitalPortResult e)
    {
        var pin = (sender as Keyboard.KeyboardKey)?.Pin as KeyboardKeyPin;
        Debug.WriteLine($"Key '{pin?.Name}' is {(e.New.State ? "down" : "up")}");
    }

    public override async Task Run()
    {
        var capsLock = _keyBoard.Pins.CapsLock.CreateDigitalOutputPort();

        var state = false;

        for (int i = 0; i < 10; i++)
        {
            state = !state;

            capsLock.State = state;

            await Task.Delay(1000);
        }
    }
}