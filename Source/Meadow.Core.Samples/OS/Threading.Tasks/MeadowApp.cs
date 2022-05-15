using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using System.Threading.Tasks;

namespace Basic_Tasks
{
    public class MeadowApp : App<F7FeatherV2, MeadowApp>
    {
        IDigitalOutputPort out1;
        IDigitalOutputPort out2;

        public MeadowApp()
        {
            out1 = Device.CreateDigitalOutputPort(Device.Pins.D00);
            out2 = Device.CreateDigitalOutputPort(Device.Pins.D01);

            out1.State = true;

            StartATask();
        }

        public void StartATask()
        {
            Task t = new Task(async () => {
                while (true) {
                    out2.State = true;
                    await Task.Delay(250);
                    out2.State = false;
                    await Task.Delay(250);
                }
            });
            t.Start();
        }
    }
}
