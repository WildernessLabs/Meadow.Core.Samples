using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using System.Threading.Tasks;

namespace Tasks_Basics
{
    public class MeadowApp : App<F7CoreComputeV2>
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
            Task t = new Task(async () =>
            {
                while (true)
                {
                    out2.State = true;
                    await Task.Delay(250);
                    out2.State = false;
                    await Task.Delay(250);
                }
            });
            t.Start();

            Task.Run(async () =>
            {
                await Task.Delay(10000);
                Resolver.Log.Info("COMPLETE");
            });

            return Task.CompletedTask;
        }

        public override Task OnShutdown()
        {
            Resolver.Log.Info("SHUTTING DOWN");
            return base.OnShutdown();
        }
    }
}