using Meadow;
using Meadow.Foundation;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.ICs.IOExpanders;

public class MeadowApp : App<Windows>
{
    private Ft232h _expander = new Ft232h();
    private St7789 _display;

    public static async Task Main(string[] _)
    {
        await MeadowOS.Start();
    }

    public override Task Initialize()
    {
        Console.WriteLine("Creating SPI Bus");

        var bus = _expander.CreateSpiBus();

        Console.WriteLine("Creating Display");

        _display = new St7789(
                    spiBus: bus,
                    chipSelectPin: _expander.Pins.C0,
                    dcPin: _expander.Pins.C1,
                    resetPin: _expander.Pins.C2,
                    width: 240, height: 240,
                    colorMode: ColorMode.Format16bppRgb565);

        return base.Initialize();
    }

    public override async Task Run()
    {
        while (true)
        {
            _display.Fill(Color.Red, true);
            await Task.Delay(1000);
            _display.Fill(Color.Green, true);
            await Task.Delay(1000);
            _display.Fill(Color.Blue, true);
            await Task.Delay(1000);
        }
    }
}