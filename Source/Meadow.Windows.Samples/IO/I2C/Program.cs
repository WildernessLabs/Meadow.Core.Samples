using Meadow;
using Meadow.Foundation.ICs.IOExpanders;
using Meadow.Foundation.Sensors.Atmospheric;
using Meadow.Foundation.Sensors.Motion;
using System.Diagnostics;

public class MeadowApp : App<Windows>
{
    private Ft232h _expander = new Ft232h();
    private Bno055 _bno;
    private Mpu6050 _mpu;
    private Bme280 _bme;

    public static async Task Main(string[] args)
    {
        await MeadowOS.Start(args);
    }

    public override Task Initialize()
    {
        Console.WriteLine("Creating Outputs");

        var bus = _expander.CreateI2cBus();

        _bme = new Bme280(bus);
        _bme.StartUpdating();

        _bme.Updated += _bme_Updated;
        return base.Initialize();
    }

    private void _bme_Updated(object? sender, IChangeResult<(Meadow.Units.Temperature? Temperature, Meadow.Units.RelativeHumidity? Humidity, Meadow.Units.Pressure? Pressure)> e)
    {
    }

    private void _mpu_TemperatureUpdated(object? sender, IChangeResult<Meadow.Units.Temperature> e)
    {
        Debug.WriteLine($"Temp: {e.New.Fahrenheit}");
    }

    private void OnEulerOrientationUpdated(object? sender, IChangeResult<Meadow.Foundation.Spatial.EulerAngles> e)
    {
        Debug.WriteLine($"Heading: {e.New.Heading.Degrees}");
    }

    public override async Task Run()
    {
    }
}