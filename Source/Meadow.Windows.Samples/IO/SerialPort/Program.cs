using Meadow;

public class MeadowApp : App<Windows>
{
    public static Task Main(string[] _)
    {
        return MeadowOS.Start();
    }

    public override Task Initialize()
    {
        //        Device.Information.

        return Task.CompletedTask;
    }

    public override async Task Run()
    {
    }
}