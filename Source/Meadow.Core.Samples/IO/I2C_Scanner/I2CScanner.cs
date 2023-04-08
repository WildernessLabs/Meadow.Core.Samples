using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using System;
using System.Collections.Generic;
using System.Linq;

namespace I2C_Scanner
{
    public class I2CScanner
    {
        private readonly IMeadowDevice _device;
        private readonly IReadOnlyList<I2cBusSpeed> _speeds;

        public I2CScanner(IMeadowDevice device)
        {
            _device = device;
            _speeds = new[] { I2cBusSpeed.Standard, I2cBusSpeed.Fast, I2cBusSpeed.FastPlus };
        }

        public I2CScanner(IMeadowDevice device, IReadOnlyList<I2cBusSpeed> speeds)
        {
            _device = device;
            _speeds = speeds;
        }

        public void VerifyAndScan()
        {
            if (!VerifyPins())
                return;

            var results = ScanBusForDevices();
            foreach (var (speed, addresses) in results)
            {
                Resolver.Log.Info($"Found {addresses.Count} devices @ {(int)speed / 1000}kHz: {string.Join(", ", addresses.Select(x => $"{x:X}"))}");
            }

            if (results.Values.All(x => x.Count == 0))
            {
                Resolver.Log.Info("No devices discovered. Please ensure the SDA and SCL pins are " +
                                  "not reversed and all devices are correctly powered. " +
                                  "Ensure all 3.3V devices are not powered by 5V " +
                                  "and no 5V devices are powered by 3.3V.");
            }
        }

        public bool VerifyPins()
        {
            var sclPin = _device is F7FeatherV2
                ? ((F7FeatherV2)_device).Pins.I2C_SCL
                : ((F7CoreComputeV2)_device).Pins.I2C1_SCL;

            var sdaPin = _device is F7FeatherV2
                ? ((F7FeatherV2)_device).Pins.I2C_SDA
                : ((F7CoreComputeV2)_device).Pins.I2C1_SDA;


            using (var scl = _device.CreateDigitalInputPort(
                pin: sclPin, 
                interruptMode: InterruptMode.EdgeFalling, 
                resistorMode: ResistorMode.InternalPullDown))
            using (var sda = _device.CreateDigitalInputPort(
                pin: sdaPin,
                interruptMode: InterruptMode.EdgeFalling,
                resistorMode: ResistorMode.InternalPullDown))
            {
                return VerifyPins(sda, scl);
            }
        }

        public static bool VerifyPins(IDigitalInputPort sda, IDigitalInputPort scl)
        {
            Resolver.Log.Info("Validating SCL and SDA pins");

            if (scl.State == false)
            {
                Resolver.Log.Info("SCL does not appear to have pull-up resistor.");
                return false;
            }

            if (sda.State == false)
            {
                Resolver.Log.Info("SDA does not appear to have pull-up resistor.");
                return false;
            }

            Resolver.Log.Info("SDA and SCL Validated Successfully.");
            return true;
        }

        public IReadOnlyDictionary<I2cBusSpeed, IReadOnlyList<byte>> ScanBusForDevices()
        {
            var results = new Dictionary<I2cBusSpeed, IReadOnlyList<byte>>();
            foreach (var speed in _speeds)
            {
                try
                {
                    Resolver.Log.Info($"Scanning I2C Bus @ {(int)speed / 1000}kHz...");

                    var bus = _device.CreateI2cBus(busSpeed:speed);
                    results.Add(speed, ScanBusForDevices(bus));

                    Resolver.Log.Info("Scanning I2C Bus complete.");
                }
                catch (Exception ex)
                {
                    Resolver.Log.Info($"An exception occurred while scanning I2C bus @ {speed}: {ex}");
                }
            }

            return results;
        }

        public static IReadOnlyList<byte> ScanBusForDevices(II2cBus bus)
        {
            var validAddresses = new List<byte>(128);
            for (byte address = 0; address < 127; address++)
            {
                if (IsReservedAddress(address))
                    continue;
                try
                {
                    var readBuffer = new Span<byte>(); 
                    readBuffer[0] = 1;
                    bus.Read(address, readBuffer);
                    validAddresses.Add(address);
                }
                catch
                {
                    // The error isn't really important here as a missing device 
                    // looks exactly like bad wiring.
                }
            }

            return validAddresses;
        }

        private static bool IsReservedAddress(byte address)
        {
            if (address >= 0x00 && address <= 0x07) 
            {
                return true;
            }
            if (address >= 0x78 && address <= 0x7F)
            {
                return true;
            }
            return false;
        }
    }
}