using Meadow;
using Meadow.Devices;
using Meadow.Gateway.WiFi;
using Meadow.Hardware;
using System;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace WiFi_Basics
{
    public class MeadowApp : App<F7FeatherV2>
    {
        private const string WIFI_NAME = "[SSID]";
        private const string WIFI_PASSWORD = "[PASSWORD]";

        public override async Task Run()
        {
            Console.WriteLine("Initialize hardware...");

            var wifi = Device.NetworkAdapters.Primary<IWiFiNetworkAdapter>();

            // connected event test.
            wifi.NetworkConnected += WiFiAdapter_NetworkConnected;

            // enumerate the public WiFi channels
            await ScanForAccessPoints(wifi);

            // connect to the wifi network.
            Console.WriteLine($"Connecting to WiFi Network {WIFI_NAME}");

            await wifi.Connect(WIFI_NAME, WIFI_PASSWORD, TimeSpan.FromSeconds(45));

            DisplayNetworkInformation();

            while (true)
            {
                await GetWebPageViaHttpClient("https://postman-echo.com/get?foo1=bar1&foo2=bar2");
            }
        }

        void WiFiAdapter_NetworkConnected(INetworkAdapter networkAdapter, NetworkConnectionEventArgs e)
        {
            Console.WriteLine("Connection request completed");
        }

        async Task ScanForAccessPoints(IWiFiNetworkAdapter wifi)
        {
            Console.WriteLine("Getting list of access points");
            var networks = await wifi.Scan(TimeSpan.FromSeconds(60));

            if (networks.Count > 0)
            {
                Console.WriteLine("|-------------------------------------------------------------|---------|");
                Console.WriteLine("|         Network Name             | RSSI |       BSSID       | Channel |");
                Console.WriteLine("|-------------------------------------------------------------|---------|");

                foreach (WifiNetwork accessPoint in networks)
                {
                    Console.WriteLine($"| {accessPoint.Ssid,-32} | {accessPoint.SignalDbStrength,4} | {accessPoint.Bssid,17} |   {accessPoint.ChannelCenterFrequency,3}   |");
                }
            }
            else
            {
                Console.WriteLine($"No access points detected");
            }
        }

        public void DisplayNetworkInformation()
        {
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();

            if (adapters.Length == 0)
            {
                Console.WriteLine("No adapters available");
            }

            foreach (NetworkInterface adapter in adapters)
            {
                IPInterfaceProperties properties = adapter.GetIPProperties();
                Console.WriteLine();
                Console.WriteLine(adapter.Description);
                Console.WriteLine(string.Empty.PadLeft(adapter.Description.Length, '='));
                Console.WriteLine($"  Adapter name: {adapter.Name}");
                Console.WriteLine($"  Interface type .......................... : {adapter.NetworkInterfaceType}");
                Console.WriteLine($"  Physical Address ........................ : {adapter.GetPhysicalAddress()}");
                Console.WriteLine($"  Operational status ...................... : {adapter.OperationalStatus}");
                
                string versions = string.Empty;

                if (adapter.Supports(NetworkInterfaceComponent.IPv4))
                {
                    versions = "IPv4";
                }

                if (adapter.Supports(NetworkInterfaceComponent.IPv6))
                {
                    if (versions.Length > 0)
                    {
                        versions += " ";
                    }
                    versions += "IPv6";
                }

                Console.WriteLine($"  IP version .............................. : {versions}");
              
                if (adapter.Supports(NetworkInterfaceComponent.IPv4))
                {
                    IPv4InterfaceProperties ipv4 = properties.GetIPv4Properties();
                    Console.WriteLine("  MTU ..................................... : {0}", ipv4.Mtu);
                }

                if ((adapter.NetworkInterfaceType == NetworkInterfaceType.Wireless80211) || (adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet))
                {
                    foreach (UnicastIPAddressInformation ip in adapter.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            Console.WriteLine($"  IP address .............................. : {ip.Address}");
                            Console.WriteLine($"  Subnet mask ............................. : {ip.IPv4Mask}");
                        }
                    }
                }
            }
        }

        public async Task GetWebPageViaHttpClient(string uri)
        {
            Console.WriteLine($"Requesting {uri} - {DateTime.Now}");

            using (HttpClient client = new HttpClient())
            {
                client.Timeout = new TimeSpan(0, 5, 0);

                HttpResponseMessage response = await client.GetAsync(uri);

                try
                {
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseBody);
                }
                catch (TaskCanceledException)
                {
                    Console.WriteLine("Request time out.");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Request went sideways: {e.Message}");
                }
            }
        }
    }
}
