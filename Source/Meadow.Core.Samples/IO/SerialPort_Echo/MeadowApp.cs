using Meadow;
using Meadow.Devices;
using System;
using System.Text;
using System.Threading;

namespace SerialEcho
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        public MeadowApp()
        {
            Console.WriteLine("+SerialEcho");

            Run();
        }

        void Run()
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

                Thread.Sleep(2000);
            }
        }
    }
}
