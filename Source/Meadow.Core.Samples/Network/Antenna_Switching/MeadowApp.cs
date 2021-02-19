using System;
using System.Threading;
using Meadow;
using Meadow.Devices;
using Meadow.Devices.Esp32.MessagePayloads;
using Meadow.Gateway.WiFi;
using Meadow.Gateways;

namespace MeadowApp
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        public MeadowApp()
        {
            Initialize();


            // enumerate the public WiFi channels
            ScanForAccessPoints();

            // get the current antenna
            Console.WriteLine($"Current antenna in use: {Device.CurrentAntenna}");

            // change to the external antenna
            Device.SetAntenna(AntennaType.External);

            // enumerate WiFis again on the new antenna
            ScanForAccessPoints();

        }

        void Initialize()
        {
            Console.WriteLine("Initialize hardware...");

            // initialize the wifi adpater
            if (!Device.InitWiFiAdapter().Result) {
                throw new Exception("Could not initialize the WiFi adapter.");
            }
        }

        protected void ScanForAccessPoints()
        {
            Console.WriteLine("Getting list of access points.");
            var networks = Device.WiFiAdapter.Scan();
            if (networks.Count > 0) {
                Console.WriteLine("|-------------------------------------------------------------|---------|");
                Console.WriteLine("|         Network Name             | RSSI |       BSSID       | Channel |");
                Console.WriteLine("|-------------------------------------------------------------|---------|");
                foreach (WifiNetwork accessPoint in networks) {
                    Console.WriteLine($"| {accessPoint.Ssid,-32} | {accessPoint.SignalDbStrength,4} | {accessPoint.Bssid,17} |   {accessPoint.ChannelCenterFrequency,3}   |");
                }
            } else {
                Console.WriteLine($"No access points detected.");
            }
        }

    }
}