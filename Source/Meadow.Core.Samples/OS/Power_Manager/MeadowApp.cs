﻿using Meadow;
using Meadow.Devices;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Watchdog
{
    public class MeadowApp : App<F7FeatherV2>
    {
        public override Task Initialize()
        {
            Resolver.Log.Info("===== Meadow Power Management Sample =====");
            Resolver.Log.Info($"{Device.Information.Platform} OS v.{Device.Information.OSVersion}");

            Device.PlatformOS.BeforeSleep += () =>
            {
                Resolver.Log.Info("Device is about to enter Sleep mode");
                // actual serial output is asynchronous, so we need to delay a little to see the output
                Thread.Sleep(500);
            };

            Device.PlatformOS.AfterWake += () =>
            {
                Resolver.Log.Info("Device has returned from Sleep mode");
            };

            Device.PlatformOS.BeforeReset += () =>
            {
                Resolver.Log.Info("Device is about to Reset");
                // actual serial output is asynchronous, so we need to delay a little to see the output
                Thread.Sleep(500);
            };

            return Task.CompletedTask;
        }

        public override async Task Run()
        {
            // we'll run a loop for 10 seconds, outputting the time
            for (var i = 0; i < 13; i++)
            {
                Resolver.Log.Info($"Time is now: {DateTime.UtcNow:HH:mm:ss}");

                await Task.Delay(TimeSpan.FromSeconds(1));

                // then we'll sleep for 5 seconds
                if (i == 9)
                {
                    Device.PlatformOS.Sleep(TimeSpan.FromSeconds(5));
                }

                // when we wake we'll output a couple more ticks
            }

            // finally we'll reset the device
            Device.PlatformOS.Reset();

            // we'll never reach here because of the reset above
        }
    }
}