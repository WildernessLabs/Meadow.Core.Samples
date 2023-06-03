using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using System;
using System.Threading.Tasks;

namespace Logging
{
    public class MeadowApp : App<F7FeatherV2>
    {
        public override Task Initialize()
        {
            Resolver.Log.Info($"Initializing...");

            // an our own logger to the system logger
            AddUdpLogger();

            return base.Initialize();
        }

        public override async Task Run()
        {
            while (true)
            {
                // prefix a random number just so we can see differences
                var r = new Random();
                Resolver.Log.Info($"Log info [{r.Next(0, 1000)}]");
                await Task.Delay(1000);
            }
        }

        private void AddUdpLogger()
        {
            var SSID = "Tacke";
            var PASSCODE = "58635863";

            Resolver.Log.Info("Connecting to network...");

            try
            {
                var wifi = Device.NetworkAdapters.Primary<IWiFiNetworkAdapter>();
                wifi.Connect(SSID, PASSCODE);

                wifi.NetworkConnected += (s, e) =>
                {
                    Resolver.Log.Info("Network connected");
                    Resolver.Log.AddProvider(new UdpLogger());
                };
            }
            catch (Exception ex)
            {
                Resolver.Log.Error($"Error connecting to network: {ex.Message}");
            }

        }

        private void AddFileLogger()
        {
            var fileLogger = new FileLogger();

            // output the log contents just for display.  Do it before adding the logger so we don't recurse
            var lineNumber = 1;
            var contents = fileLogger.GetLogContents();
            if (contents.Length > 0)
            {
                Resolver.Log.Info($"Log contents{Environment.NewLine}------------");

                foreach (var line in contents)
                {
                    Resolver.Log.Info($"{lineNumber++:000}> {line}");
                }
                Resolver.Log.Info($"------------");
            }
            else
            {
                Resolver.Log.Info($"Log is empty");
            }
            Resolver.Log.AddProvider(fileLogger);
        }
    }
}