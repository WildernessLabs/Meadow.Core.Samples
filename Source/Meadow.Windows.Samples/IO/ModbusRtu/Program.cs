using Meadow;
using Meadow.Foundation.Thermostats;
using Meadow.Modbus;

public class MeadowApp : App<Windows>
{
    public static async Task Main(string[] _)
    {
        await MeadowOS.Start();
    }

    private Tstat8 _thermostat;

    public override Task Initialize()
    {
        // use the first one - adjust to your needs
        var port = Device.CreateSerialPort("COM4", 19200);
        port.ReadTimeout = TimeSpan.FromSeconds(2);
        var rtu = new ModbusRtuClient(port);
        rtu.Connect();

        _thermostat = new Tstat8(rtu, 201);

        return Task.CompletedTask;
    }

    public override async Task Run()
    {
        while (true)
        {
            try
            {
                var temp = await _thermostat.GetCurrentTemperature();

                Console.WriteLine($"Current temperature: {temp.Fahrenheit}F");
            }
            catch
            {
            }

            await Task.Delay(1000);
        }
    }
}