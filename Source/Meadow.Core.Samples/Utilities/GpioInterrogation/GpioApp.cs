﻿#nullable enable

using System;
using System.Text;
using Meadow;
using Meadow.Devices;
using Meadow.Hardware;

namespace GpioInterrogation
{
    public class GpioApp : App<F7Micro, GpioApp>
    {
        public GpioApp()
        {
            // uncomment to build out a table of pins functions
            StringBuilder table = new StringBuilder();
            table.Append(BuildHeader());
            foreach (var pin in Device.Pins.AllPins) {
                table.Append(BuildRow(pin));
            }
            Console.WriteLine(table.ToString());

            //foreach (var pin in Device.Pins.AllPins) {
            //    Console.WriteLine($"Found pin: {pin.Name}");
            //    foreach (var channel in pin.SupportedChannels) {
            //        Console.WriteLine($"Contains a {channel.GetType()} channel called: {channel.Name}.");
            //    }
            //}
        }

        protected string BuildHeader()
        {
            return "| Meadow Pin Name | MCU Pin Name | Digital Channel | Analog Channel | PWM Timer Channel | Interrupt Group | \r\n" +
                   "|-----------------|--------------|-----------------|----------------|-------------------|-----------------| \r\n";
        }

        protected string BuildRow(IPin pin)
        {
            StringBuilder stringBuilder = new StringBuilder("| ");

            stringBuilder.Append($"{pin.Name} | {pin.Key.ToString()} | ");

            IUartChannelInfo? uartChan = null;
            ICanChannelInfo? canChan = null;
            IPwmChannelInfo? pwmChan = null;
            II2cChannelInfo? i2cChan = null;
            ISpiChannelInfo? spiChan = null;
            IAnalogChannelInfo? analogChan = null;
            IDigitalChannelInfo? digitalChan = null;

            foreach (var chan in pin.SupportedChannels) {

                switch (chan) {
                    case IUartChannelInfo u:
                        uartChan = u;
                        break;
                    case ICanChannelInfo c:
                        canChan = c;
                        break;
                    case IPwmChannelInfo p:
                        pwmChan = p;
                        break;
                    case II2cChannelInfo i:
                        i2cChan = i;
                        break;
                    case ISpiChannelInfo s:
                        spiChan = s;
                        break;
                    case IAnalogChannelInfo a:
                        analogChan = a;
                        break;
                    case IDigitalChannelInfo d:
                        digitalChan = d;
                        break;
                    default:
                        break;
                }
            }

            stringBuilder.Append($" { ((digitalChan == null) ? "n/a" : digitalChan?.Name)} |");
            stringBuilder.Append($" { ((analogChan == null) ? "n/a" : analogChan?.Name)} |");
            stringBuilder.Append($" { ((pwmChan == null) ? "n/a" : pwmChan?.TimerChannel.ToString())} |");
            stringBuilder.Append($" { ((digitalChan == null) ? "n/a" : digitalChan?.InterruptGroup?.ToString())} |");
            stringBuilder.Append("\r\n");
            return stringBuilder.ToString();
        }
    }
}