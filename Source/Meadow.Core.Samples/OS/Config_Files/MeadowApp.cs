using Meadow;
using Meadow.Devices;
using System;
using System.Threading.Tasks;

namespace MeadowApp
{
    public class MeadowApp : App<F7MicroV2, MeadowApp>
    {
        public MeadowApp()
        {
            Initialize();

            Console.WriteLine($"Machine Name: {Environment.MachineName}");

            StartHeartbeat();

            OutputDeviceInfo();
            OutputMeadowOSInfo();

            OutputDeviceConfigurationInfo();
        }

        void Initialize()
        {
            if (Device.WiFiAdapter.IsConnected) 
            {
                Console.WriteLine("WiFi adapter already connected.");
            }
            else 
            {
                Console.WriteLine("WiFi adapter not connected.");
                Device.WiFiAdapter.WiFiConnected += (s, e) => 
                {
                    Console.WriteLine("WiFi adapter connected.");
                };
            }
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
                Console.WriteLine($"====================OutputDeviceConfigurationInfo======================");
                Console.WriteLine($"Automatically connect to network: {Device.WiFiAdapter.AutomaticallyStartNetwork}");
                Console.WriteLine($"Automatically reconnect: {Device.WiFiAdapter.AutomaticallyReconnect}");
                Console.WriteLine($"Get time at startup: {Device.WiFiAdapter.GetNetworkTimeAtStartup}");
                //Console.WriteLine($"NTP Server: {Device.WiFiAdapter.NtpServer}");
                Console.WriteLine($"Default access point: {Device.WiFiAdapter.DefaultAcessPoint}");
                Console.WriteLine($"Maximum retry count: {Device.WiFiAdapter.MaximumRetryCount}");
                Console.WriteLine($"MAC address: {FormatMacAddressString(Device.WiFiAdapter.MacAddress)}");
                Console.WriteLine($"Soft AP MAC address: {FormatMacAddressString(Device.WiFiAdapter.ApMacAddress)}");
                Console.WriteLine($"=======================================================================");
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
                    Console.WriteLine($"{DateTime.Now} {Device.WiFiAdapter.IpAddress}");
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