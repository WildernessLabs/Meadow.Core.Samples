using Meadow;
using Meadow.Foundation.ICs.IOExpanders;
using Meadow.Foundation.Sensors.Motion;
using System.Diagnostics;

public class MeadowApp : App<Windows>
{
    private Ft232h _expander = new Ft232h();
    private Bno055 _bno;
    private Mpu6050 _mpu;

    public static async Task Main(string[] args)
    {
        await MeadowOS.Start(args);
    }

    public override Task Initialize()
    {
        Console.WriteLine("Creating Outputs");

        var bus = _expander.CreateI2cBus();

        _mpu = new Mpu6050(bus);
        _mpu.TemperatureUpdated += _mpu_TemperatureUpdated;
        _mpu.StartUpdating();

        //        var _bno = new Bno055(bus);
        //        _bno.EulerOrientationUpdated += OnEulerOrientationUpdated;
        //_bno.StartUpdating();

        return base.Initialize();
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