using Meadow;
using Meadow.Foundation.ICs.IOExpanders;
using Meadow.Hardware;

public class MeadowApp : App<Windows>
{
    private Ft232h _expander = new Ft232h();
    private II2cBus _i2c;
    private ISpiBus _spi;

    public static Task Main(string[] _)
    {
        return MeadowOS.Start();
    }

    public override Task Initialize()
    {
        Console.WriteLine("Creating Outputs");

        _i2c = _expander.CreateI2cBus();
        _spi = _expander.CreateSpiBus();

        _expander.CreateDigitalInputPort(_expander.Pins.C0);

        return Task.CompletedTask;
    }

    public override async Task Run()
    {
    }
}