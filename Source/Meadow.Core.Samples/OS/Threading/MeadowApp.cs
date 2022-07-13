using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using System.Threading;
using System.Threading.Tasks;

namespace Threading_Basics
{
    public class MeadowApp : App<F7FeatherV2>
    {
        IDigitalOutputPort out1;
        IDigitalOutputPort out2;

        public override Task Initialize()
        {
            out1 = Device.CreateDigitalOutputPort(Device.Pins.D00);
            out2 = Device.CreateDigitalOutputPort(Device.Pins.D01);

            out1.State = true;

            return Task.CompletedTask;
        }

        public override Task Run()
        {
            Thread th = new Thread(() =>
            {
                while (true)
                {
                    out2.State = true;
                    Thread.Sleep(250);
                    out2.State = false;
                    Thread.Sleep(250);
                }
            });
            th.Start();

            return Task.CompletedTask;
        }
    }
}