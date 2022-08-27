using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using System;
using System.Threading.Tasks;

namespace Config_Files
{
    public class MeadowApp : App<F7FeatherV2>
    {
        INetworkAdapter wifi;
        private bool isF7PlatformOs;

        public override Task Initialize()
        {
            wifi = Device.NetworkAdapters.Primary<IWiFiNetworkAdapter>();
            if (wifi.IsConnected)
            {
                Console.WriteLine("WiFi adapter already connected.");
            }
            else
            {
                Console.WriteLine("WiFi adapter not connected.");
                wifi.NetworkConnected += (s, e) =>
                {
                    Console.WriteLine("WiFi adapter connected.");
                };
            }

            return Task.CompletedTask;
        }

        public override Task Run()
        {
            StartHeartbeat();

            OutputDeviceInfo();
            OutputMeadowOSInfo();

            OutputDeviceConfigurationInfo();

            return Task.CompletedTask;
        }

        void OutputDeviceInfo()
        {
            Console.WriteLine($"=========================OutputDeviceInfo==============================");
            Console.WriteLine($"Device name: {Device.Information.DeviceName}");
            Console.WriteLine($"Processor serial number: {Device.Information.ProcessorSerialNumber}");
            Console.WriteLine($"Processor ID: {Device.Information.ChipID}");
            Console.WriteLine($"Model: {Device.Information.Model}");
            Console.WriteLine($"Processor type: {Device.Information.ProcessorType}");
            Console.WriteLine($"Product: {Device.Information.Model}");
            Console.WriteLine($"Coprocessor type: {Device.Information.CoprocessorType}");
            Console.WriteLine($"Coprocessor firmware version: {Device.Information.CoprocessorOSVersion}");
            Console.WriteLine($"=======================================================================");
        }

        void OutputMeadowOSInfo()
        {
            Console.WriteLine($"=========================OutputMeadowOSInfo============================");
            Console.WriteLine($"OS version: {MeadowOS.SystemInformation.OSVersion}");
            Console.WriteLine($"Mono version: {MeadowOS.SystemInformation.MonoVersion}");
            Console.WriteLine($"Build date: {MeadowOS.SystemInformation.OSBuildDate}");
            Console.WriteLine($"=======================================================================");
        }

        void OutputDeviceConfigurationInfo()
        {
            try
            {
                var isF7PlatformOS = Device.PlatformOS is F7PlatformOS;
                var esp32Wifi = wifi as Esp32Coprocessor;
                if (isF7PlatformOS && esp32Wifi != null)
                {
                    Console.WriteLine($"====================OutputDeviceConfigurationInfo======================");
                    Console.WriteLine($"Automatically connect to network: {(Device.NetworkAdapters.Primary<IWiFiNetworkAdapter>() as Esp32Coprocessor)?.AutoConnect}");
                    Console.WriteLine($"Automatically reconnect: {esp32Wifi.AutoReconnect}");
                    Console.WriteLine($"Get time at startup: {F7PlatformOS.GetBoolean(IPlatformOS.ConfigurationValues.AutomaticallyStartNetwork)}");
                    //Console.WriteLine($"NTP Server: {Device.WiFiAdapter.NtpServer}");
                    Console.WriteLine($"Default access point: {esp32Wifi.DefaultSsid}");
                    Console.WriteLine($"Maximum retry count: {esp32Wifi.MaximumRetryCount}");
                    Console.WriteLine($"MAC address: {FormatMacAddressString(esp32Wifi.MacAddress.GetAddressBytes())}");
                    Console.WriteLine($"Soft AP MAC address: {FormatMacAddressString(esp32Wifi.ApMacAddress.GetAddressBytes())}");
                    Console.WriteLine($"=======================================================================");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        protected void StartHeartbeat()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    Console.WriteLine($"{DateTime.Now} {wifi.IpAddress}");
                    await Task.Delay(10000);
                }
            });
        }

        protected string FormatMacAddressString(byte[] address)
        {
            string result = string.Empty;
            for (int index = 0; index < address.Length; index++)
            {
                result += address[index].ToString("X2");
                if (index != (address.Length - 1))
                {
                    result += ":";
                }
            }
            return (result);
        }
    }
}