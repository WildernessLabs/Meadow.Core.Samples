using System;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Meadow;
using Meadow.Devices;
using Meadow.Gateway.WiFi;
using Meadow.Hardware;
using System.IO;

namespace WiFi_Basics
{
    // **** WINDOWS USERS ****
    // due to a VSWindows bug that links out the System.Net.Http.dll, you must
    // manually edit the .csproj file, uncomment the post-build step, and
    // change the [your user] text to your username.
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        public MeadowApp()
        {
            Initialize();


            Console.WriteLine($"Connecting to WiFi Network {Secrets.WIFI_NAME}");

            var result = Device.WiFiAdapter.Connect(Secrets.WIFI_NAME, Secrets.WIFI_PASSWORD);
            if (result.ConnectionStatus != ConnectionStatus.Success) { 
            //if (Device.WiFiAdapter.Connect(Secrets.WIFI_NAME, Secrets.WIFI_PASSWORD).ConnectionStatus != ConnectionStatus.Success) {
                throw new Exception($"Cannot connect to network: {result.ConnectionStatus}");
            }
            Console.WriteLine("Connection request completed.");


            ScanForAccessPoints();

            GetWebPageViaHttpClient("http://postman-echo.com/get?foo1=bar1&foo2=bar2").Wait();

        }

        void Initialize()
        {
            Console.WriteLine("Initialize hardware...");

            Device.InitWiFiAdapter().Wait();
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

        public async Task GetWebPageViaHttpClient(string uri)
        {
            Console.WriteLine($"Requesting {uri}");

            using (HttpClient client = new HttpClient()) {
                client.Timeout = new TimeSpan(0, 5, 0);

                HttpResponseMessage response = await client.GetAsync(uri);

                try {
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseBody);
                } catch (TaskCanceledException) {
                    Console.WriteLine("Request time out.");
                } catch (Exception e) {
                    Console.WriteLine($"Request went sideways: {e.Message}");
                }
            }
        }

    }
}