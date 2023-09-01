using Meadow;
using Meadow.Devices;
using System.Threading.Tasks;

namespace OS_Telemetry;

public class MeadowApp : App<F7FeatherV1>
{
    public override async Task Run()
    {
        Resolver.Log.Info("===== Meadow OS Telemetry =====");

        while (true)
        {
            // This is F7-specific
            var memoryInfo = (Device.PlatformOS as F7PlatformOS)?.GetMemoryAllocationInfo();

            if (memoryInfo.HasValue)
            {
                Resolver.Log.Info($" Memory");
                Resolver.Log.Info($"   Total memory: {memoryInfo.Value.Arena}");
                Resolver.Log.Info($"   Total allocated: {memoryInfo.Value.TotalAllocated}");
                Resolver.Log.Info($"   Total free: {memoryInfo.Value.TotalFree}");
            }

            var load = (Device.PlatformOS as F7PlatformOS)?.ProcessorLoad();
            Resolver.Log.Info($" Processor");
            Resolver.Log.Info($"   Usage: {load}%");

            await Task.Delay(5000);
        }
    }
}