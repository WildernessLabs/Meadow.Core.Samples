using Meadow;
using Meadow.Devices;
using Meadow.Gateway.WiFi;
using Meadow.Hardware;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace WiFi_Basics
{
    public class MeadowApp : App<F7FeatherV2>
    {
        public override async Task Run()
        {
            Console.WriteLine("Initialize hardware...");

            var wifi = Device.NetworkAdapters.Primary<IWiFiNetworkAdapter>();

            // connected event test.
            wifi.NetworkConnected += WiFiAdapter_NetworkConnected;

            // enumerate the public WiFi channels
            await ScanForAccessPoints(wifi);

            // connnect to the wifi network.
            Console.WriteLine($"Connecting to WiFi Network {Secrets.WIFI_NAME}");

            var connectionResult = await wifi.Connect(Secrets.WIFI_NAME, Secrets.WIFI_PASSWORD, TimeSpan.FromSeconds(45));

            if(connectionResult.ConnectionStatus != ConnectionStatus.Success)
            {
                throw new Exception($"Cannot connect to network: {connectionResult.ConnectionStatus}");
            }
            else
            {
                Console.WriteLine($"IP Address: {wifi.IpAddress}");
                Console.WriteLine($"Subnet mask: {wifi.SubnetMask}");
                Console.WriteLine($"Gateway: {wifi.Gateway}");
            }

            do
            {
                GetWebPageViaHttpClient("https://postman-echo.com/get?foo1=bar1&foo2=bar2").Wait();
            }
            while(true);
        }

        void WiFiAdapter_NetworkConnected(object sender, EventArgs e)
        {
            Console.WriteLine("Connection request completed.");
        }

        async Task ScanForAccessPoints(IWiFiNetworkAdapter wifi)
        {
            Console.WriteLine("Getting list of access points.");
            var networks = await wifi.Scan(TimeSpan.FromSeconds(60));

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

        public async Task GetWebPageViaHttpClient(string uri)
        {
            Console.WriteLine($"Requesting {uri} - {DateTime.Now}");

            using(HttpClient client = new HttpClient())
            {
                client.Timeout = new TimeSpan(0, 5, 0);

                HttpResponseMessage response = await client.GetAsync(uri);

                try
                {
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseBody);
                }
                catch(TaskCanceledException)
                {
                    Console.WriteLine("Request time out.");
                }
                catch(Exception e)
                {
                    Console.WriteLine($"Request went sideways: {e.Message}");
                }
            }
        }
    }
}