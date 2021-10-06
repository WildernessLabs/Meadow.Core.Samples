using System;
using System.Threading;
using Meadow;
using Meadow.Devices;

namespace MeadowApp
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        public MeadowApp()
        {
            Initialize();

            Console.WriteLine($"Machine Name: {System.Environment.MachineName}");

            OutputDeviceInfo();
        }

        void Initialize()
        {
            Console.WriteLine("Initialize hardware...");

        }

        void OutputDeviceInfo()
        {
            F7Micro.DeviceInformation information = Device.GetDeviceInformation();

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
    }
}
