using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace Update_Sample
{
    public class MeadowApp : App<F7FeatherV2>
    {
        private Stopwatch _stopWatch;

        public override Task Initialize()
        {
            Resolver.UpdateService.OnUpdateAvailable += (s, e) =>
            {
                Resolver.Log.Info($"An {e.UpdateType} update is available! Version: {e.Version} Size: {e.DownloadSize}");

                Resolver.Log.Info("Retrieving update...");
                _stopWatch = Stopwatch.StartNew();
                Resolver.UpdateService.RetrieveUpdate(e);
            };

            Resolver.UpdateService.OnUpdateRetrieved += async (s, e) =>
            {
                _stopWatch.Stop();
                Resolver.Log.Info($"Update {e.Version} has been retrieved, which took {_stopWatch.Elapsed.TotalSeconds} seconds.");

                // wait a little while to allow us to see output, etc.
                await Task.Delay(TimeSpan.FromSeconds(5));

                Resolver.Log.Info("Applying update...");
                Resolver.UpdateService.ApplyUpdate(e);
            };

            return Task.CompletedTask;
        }

        public override async Task Run(CancellationToken cancellationToken)
        {
            var wifi = Resolver.Device.NetworkAdapters.Primary<IWiFiNetworkAdapter>();

            wifi.NetworkConnected += (s, e) =>
            {
                Resolver.Log.Info("Network connected!");

                //                Task.Run(() => ServerPingProc());
            };

            Resolver.Log.Info("Connecting to the network...");
            await wifi.Connect("{my ssid}", "{my passphrase}");
        }

        private async Task ServerPingProc()
        {
            var ping = new Ping();

            while (true)
            {
                try
                {
                    var result = ping.Send("192.168.1.133");

                    switch (result.Status)
                    {
                        case IPStatus.Success:
                            Resolver.Log.Info($"Ping response in: {result.RoundtripTime} ms");
                            break;
                        default:
                            Resolver.Log.Info($"Ping failed: {result.Status}");
                            break;
                    }
                }
                catch (Exception e)
                {
                    Resolver.Log.Warn($"Ping failed: {e.Message}");
                }

                await Task.Delay(TimeSpan.FromSeconds(5));
            }
        }
    }
}