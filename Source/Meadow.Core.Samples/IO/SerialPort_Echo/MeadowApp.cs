using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using System;
using System.Text;
using System.Threading.Tasks;

namespace SerialPort_Echo
{
    public class MeadowApp : App<F7FeatherV2>
    {
        ISerialPort port;

        public override Task Initialize()
        {
            Console.WriteLine("Using 'Com1'...");
            var port = Device.CreateSerialPort(Device.SerialPortNames.Com1, 115200);
            Console.WriteLine("\tCreated");
            port.Open();
            if (port.IsOpen)
            {
                Console.WriteLine("\tOpened");
            }
            else
            {
                Console.WriteLine("\tFailed to Open");
            }

            return Task.CompletedTask;
        }

        public override async Task Run()
        {
            var buffer = new byte[1024];

            while (true)
            {
                Console.WriteLine("Writing data...");
                port.Write(Encoding.ASCII.GetBytes("Hello Meadow!"));

                var dataLength = port.BytesToRead;
                var read = port.Read(buffer, 0, dataLength);

                if (read == 0)
                {
                    Console.WriteLine($"Read {read} bytes");
                }
                else
                {
                    Console.WriteLine($"Read {read} bytes: {BitConverter.ToString(buffer, 0, read)}");
                }

                await Task.Delay(2000);
            }
        }
    }
}