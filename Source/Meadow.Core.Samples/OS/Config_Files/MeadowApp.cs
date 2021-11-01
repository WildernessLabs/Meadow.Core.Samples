using System;
using System.Threading;
using System.Threading.Tasks;
using Meadow;
using Meadow.Devices;

namespace MeadowApp
{
    public class MeadowApp : App<F7MicroV2, MeadowApp>
    {
        public MeadowApp()
        {
            Initialize();

            Console.WriteLine($"Machine Name: {System.Environment.MachineName}");

            StartHeartbeat();

            OutputDeviceInfo();

            OutputDeviceConfigurationInfo();
        }

        void Initialize()
        {
            if (Device.WiFiAdapter.IsConnected) {
                Console.WriteLine("WiFi adapter already connected.");
            } else {
                Console.WriteLine("WiFi adapter not connected.");

                Device.WiFiAdapter.WiFiConnected += (s, e) => {
                    Console.WriteLine("WiFi adapter connected.");
                };
            }
        }

        void OutputDeviceInfo()
        {
            var information = Device.GetDeviceInformation();

            Console.WriteLine($"Device name: {information.DeviceName}");
            Console.WriteLine($"OS version: {information.OsVersion}");
            Console.WriteLine($"Microcontroller serial number: {information.SerialNumber}");
            Console.WriteLine($"Microcontroller ID: {information.UniqueId}");
            Console.WriteLine($"Model: {information.Model}");
            Console.WriteLine($"Mono version: {information.MonoVersion}");
            Console.WriteLine($"Processor type: {information.ProcessorType}");
            Console.WriteLine($"Product: {information.Product}");
            Console.WriteLine($"Build date: {information.BuildDate}");
            Console.WriteLine($"Coprocessor type: {information.CoprocessorType}");
            Console.WriteLine($"Coprocessor firmware version: {information.CoprocessorFirmwareVersion}");
        }

        void OutputDeviceConfigurationInfo()
        {
            try {

                bool autmaticallStartNetwork = Device.WiFiAdapter.AutomaticallyStartNetwork;
                Console.WriteLine($"Automatically connect to network: {autmaticallStartNetwork}");

                bool automaticallyReconnect = Device.WiFiAdapter.AutomaticallyReconnect;
                Console.WriteLine($"Automatically reconnect: {automaticallyReconnect}");

                bool getTimeAtStartup = Device.WiFiAdapter.GetNetworkTimeAtStartup;
                Console.WriteLine($"Get time at startup: {getTimeAtStartup}");
                //Console.WriteLine($"NTP Server: {Device.WiFiAdapter.NtpServer}");

                Console.WriteLine($"Default access point: {Device.WiFiAdapter.DefaultAcessPoint}");

                uint maximumRetryCount = Device.WiFiAdapter.MaximumRetryCount;
                Console.WriteLine($"Maximum retry count: {maximumRetryCount}");

                //Console.WriteLine($"MAC address: {MacAddressString(Device.WiFiAdapter.MacAddress)}");
                //Console.WriteLine($"Soft AP MAC address: {MacAddressString(Device.WiFiAdapter.ApMacAddress)}");

            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }

        }

        protected void StartHeartbeat()
        {
            Task.Run(async () => {
                while (true) {
                    Console.WriteLine("Beep...");
                    await Task.Delay(5000);
                }
            });
        }
    }
}
