using Meadow;
using Meadow.Devices;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BatteryLevel
{
    public class MeadowApp : App<F7FeatherV2>
    {
        public override Task Run()
        {
            while (true) {

                Console.WriteLine($"Battery Voltage: {Device.GetBatteryInfo().Voltage.Value.Volts:N2}V");

                Thread.Sleep(3000);
            }
        }
    }
}