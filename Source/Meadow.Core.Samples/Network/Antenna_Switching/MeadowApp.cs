using Meadow;
using Meadow.Devices;
using Meadow.Gateway.WiFi;
using Meadow.Hardware;
using System;
using System.Threading.Tasks;
namespace Antenna_Switching
{
    public class MeadowApp : App<F7FeatherV2>
    {
        public override Task Initialize()
        {
            Console.WriteLine("Initialize hardware...");

            return Task.CompletedTask;
        }

        public override async Task Run()
        {
            var wifi = Device.NetworkAdapters.Primary<IWiFiNetworkAdapter>();

            // enumerate the public WiFi channels
            await ScanForAccessPoints(wifi);

            // get the current antenna
            Console.WriteLine($"Current antenna in use: {wifi.CurrentAntenna}");

            // change to the external antenna
            Console.WriteLine($"Switching to external antenna.");
            wifi.SetAntenna(AntennaType.External, persist: false);
            Console.WriteLine($"Current antenna in use: {wifi.CurrentAntenna}");

            // enumerate WiFis again on the new antenna
            await ScanForAccessPoints(wifi);
        }

        async Task ScanForAccessPoints(IWiFiNetworkAdapter adapter)
        {
            Console.WriteLine("Getting list of access points.");

            var networks = await adapter.Scan();
            if(networks.Count > 0)
            {
                Console.WriteLine("|-------------------------------------------------------------|---------|");
                Console.WriteLine("|         Network Name             | RSSI |       BSSID       | Channel |");
                Console.WriteLine("|-------------------------------------------------------------|---------|");
                foreach(WifiNetwork accessPoint in networks)
                {
                    Console.WriteLine($"| {accessPoint.Ssid,-32} | {accessPoint.SignalDbStrength,4} | {accessPoint.Bssid,17} |   {accessPoint.ChannelCenterFrequency,3}   |");
                }
            }
            else
            {
                Console.WriteLine($"No access points detected.");
            }
        }
    }
}