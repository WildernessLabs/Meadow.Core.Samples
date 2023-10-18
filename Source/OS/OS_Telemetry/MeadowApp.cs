using Meadow;
using Meadow.Devices;
using System;
using System.Linq;
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
            var gcAlloc = GC.GetTotalMemory(false);

            if (memoryInfo.HasValue)
            {
                Resolver.Log.Info($" Memory");
                Resolver.Log.Info($"   Total memory: {memoryInfo.Value.Arena:n0}");
                Resolver.Log.Info($"   Total allocated: {memoryInfo.Value.TotalAllocated:n0}");
                Resolver.Log.Info($"   Total free: {memoryInfo.Value.TotalFree:n0}");
                Resolver.Log.Info($"   GC Allocated: {gcAlloc:n0}");
            }


            var load = Device.PlatformOS?.GetProcessorUtilization().Average();
            Resolver.Log.Info($" Processor");
            Resolver.Log.Info($"   Usage: {load}%");

            var storage = Device.PlatformOS?.GetStorageInformation();
            Resolver.Log.Info($" Storage");
            Resolver.Log.Info($"   {storage[0].SpaceAvailable.MegaBytes:0.00}/{storage[0].Size.MegaBytes:0.0}MB");
            Resolver.Log.Info($"\r\n");

            await Task.Delay(5000);
        }
    }
}