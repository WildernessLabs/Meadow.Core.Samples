using Meadow;
using Meadow.Devices;
using System;
using System.Threading;

namespace MeadowApp
{
    public class MeadowApp : App<F7FeatherV2, MeadowApp>
    {
        public MeadowApp()
        {
            Initialize();
        }

        void Initialize()
        {
            Console.WriteLine("Initialize hardware...");

            // enable the watchdog for 10s
            MeadowOS.CurrentDevice.WatchdogEnable(new TimeSpan(10000));
            StartPettingWatchdog(9000);
        }

        /// <summary>
        /// Starts up a thread that resets the watchdog at the specified interval.
        /// </summary>
        /// <param name="pettingInterval"></param>
        void StartPettingWatchdog(int pettingInterval)
        {
            // just for good measure, let's reset the watchdog to begin with
            MeadowOS.CurrentDevice.WatchdogReset();
            // start a thread that pets it
            Thread t = new Thread(() => {
                while (true) {
                    Thread.Sleep(pettingInterval);
                    Console.WriteLine("Petting watchdog.");
                    MeadowOS.CurrentDevice.WatchdogReset();
                }
            });
            t.Start();
        }
    }
}