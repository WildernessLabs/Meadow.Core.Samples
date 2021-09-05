﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Leds;

namespace MeadowApp
{
    // this sample illustrates how to use the `BeginInvokeOnMainThread()` method
    // to ensure that code gets queued to run on the main execution thread.
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        RgbPwmLed onboardLed;

        public MeadowApp()
        {
            Initialize();

            onboardLed.SetColor(WildernessLabsColors.PearGreen);
            Thread.Sleep(1000);

            Task.Run(() => {
                MeadowOS.BeginInvokeOnMainThread(
                    () => {
                        for (int i = 0; i < 10; i++) {
                            onboardLed.SetColor(WildernessLabsColors.AzureBlue);
                            Thread.Sleep(1000);
                            onboardLed.SetColor(WildernessLabsColors.ChileanFire);
                            Thread.Sleep(1000);
                        }
                    });
            });
        }

        void Initialize()
        {
            Console.WriteLine("Initialize hardware...");

            onboardLed = new RgbPwmLed(device: Device,
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue,
                3.3f, 3.3f, 3.3f,
                Meadow.Peripherals.Leds.IRgbLed.CommonType.CommonAnode);
        }

    }
}