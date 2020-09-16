using System;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Meadow;
using Meadow.Devices;
using Meadow.Gateway.WiFi;
using Meadow.Hardware;

namespace WiFi_Basics
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        public MeadowApp()
        {
            Initialize();

            //Device.WiFiAdapterInitilaized += (s,e) => {

                Console.WriteLine($"Connecting to WiFi Network {Secrets.WIFI_NAME}");

                if (Device.WiFiAdapter.Connect(Secrets.WIFI_NAME, Secrets.WIFI_PASSWORD).ConnectionStatus != ConnectionStatus.Success) {
                    throw new Exception("Cannot connect to network, applicaiton halted.");
                }
                Console.WriteLine("Connection request completed.");

                //throw new Exception("Test exception");

                ScanForAccessPoints();

                GetWebPageAsync("http://postman-echo.com/get?foo1=bar1&foo2=bar2").Wait();

            //};
        }

        void Initialize()
        {
            Console.WriteLine("Initialize hardware...");

            Device.InitWiFiAdapter().Wait();
        }

        protected void ScanForAccessPoints()
        {
            Console.WriteLine("Getting list of access points.");
            Device.WiFiAdapter.Scan();
            if (Device.WiFiAdapter.Networks.Count > 0) {
                Console.WriteLine("|-------------------------------------------------------------|---------|");
                Console.WriteLine("|         Network Name             | RSSI |       BSSID       | Channel |");
                Console.WriteLine("|-------------------------------------------------------------|---------|");
                foreach (WifiNetwork accessPoint in Device.WiFiAdapter.Networks) {
                    Console.WriteLine($"| {accessPoint.Ssid,-32} | {accessPoint.SignalDbStrength,4} | {accessPoint.Bssid,17} |   {accessPoint.ChannelCenterFrequency,3}   |");
                }
            } else {
                Console.WriteLine($"No access points detected.");
            }
        }

        public async Task GetWebPageAsync(string uri)
        {
            Console.WriteLine($"Requesting {uri}");

            HttpClient client = new HttpClient();
            client.Timeout = new TimeSpan(0, 5, 0);

            HttpResponseMessage response = await client.GetAsync(uri);

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseBody);
        }

    }
}