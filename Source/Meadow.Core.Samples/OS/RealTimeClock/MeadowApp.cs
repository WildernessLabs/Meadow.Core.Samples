using Meadow;
using Meadow.Devices;
using System;
using System.Threading.Tasks;

namespace RealTimeClock
{
    public class MeadowApp : App<F7FeatherV2>
    {
        public override Task Run()
        {
            //set current time to 12pm on March 20, 2020
            Console.WriteLine("Hello RTC");
            
            Device.SetClock(new DateTime(2020, 3, 22, 12, 0, 0));

            Console.WriteLine($"Today is: {DateTime.Now}");

            return Task.CompletedTask;
        }
    }
}