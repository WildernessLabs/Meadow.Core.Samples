using Meadow;

public class MeadowApp : App<Windows>
{
    public static async Task Main(string[] _)
    {
        await MeadowOS.Start();
    }

    public override Task Initialize()
    {
        var names = Device.PlatformOS.GetSerialPortNames();

        // use the first one - adjust to your needs
        var port = Device.CreateSerialPort(names.First());
        port.Open();


        return Task.CompletedTask;
    }

    public override async Task Run()
    {
    }
}