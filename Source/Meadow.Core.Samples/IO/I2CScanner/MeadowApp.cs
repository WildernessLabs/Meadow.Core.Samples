using System;
using System.Threading.Tasks;
using Meadow;
using Meadow.Devices;

namespace I2CScanner
{
    public class MeadowApp : App<F7FeatherV2>
    {
        public override Task Initialize()
        {
            Resolver.Log.Info("Initializing...");

            var scanner = new I2CScanner(Device);
            scanner.VerifyAndScan();

            return base.Initialize();
        }
    }
}