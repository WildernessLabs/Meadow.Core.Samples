using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Leds;
using Meadow.Hardware;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace WiFi_Test
{
    public class MeadowApp : App<F7FeatherV2>
    {
        private const string SSID = "SSID";
        private const string PASSWORD = "PASSWORD";

        public override async Task Initialize()
        {
            var rgbPwmLed = new RgbPwmLed(
                Device.Pins.OnboardLedRed,
                Device.Pins.OnboardLedGreen,
                Device.Pins.OnboardLedBlue);
            rgbPwmLed.SetColor(Color.Red);

            try
            {
                Resolver.Log.Info("Initialize WiFi adapter...");

                var wifi = Device.NetworkAdapters.Primary<IWiFiNetworkAdapter>();
                wifi.NetworkConnected += NetworkConnected;

                Resolver.Log.Info("Scanning for networks...");
                foreach (var network in await wifi.Scan())
                {
                    Resolver.Log.Info($"Found {network.Ssid}, signal strength: {network.SignalDbStrength}dB");
                }

                Resolver.Log.Info($"Connecting to {SSID}...");
                await wifi.Connect(SSID, PASSWORD);
            }
            catch (Exception ex)
            {
                rgbPwmLed.SetColor(Color.Red);
                Resolver.Log.Info(ex.Message);
            }

            rgbPwmLed.SetColor(Color.Green);
        }

        private async void NetworkConnected(INetworkAdapter sender, NetworkConnectionEventArgs args)
        {
            Resolver.Log.Info("Network connected...");

            while (true)
            {
                try
                {
                    Resolver.Log.Info("Getting the weather...");

                    using var client = new HttpClient();
                    client.DefaultRequestHeaders.Add("User-Agent", "curl");
                    var weather = await client.GetStringAsync("http://wttr.in/?format=%l%20%t%20%C");

                    Resolver.Log.Info(weather);
                    
                    await Task.Delay(5000);
                }
                catch (Exception ex)
                {
                    Resolver.Log.Info(ex.Message);
                }
            }
        }
    }
}