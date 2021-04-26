using System;
using System.Threading;
using Meadow;
using Meadow.Devices;
//using Meadow.Foundation;
//using Meadow.Foundation.Leds;
using Meadow.Gateways.Bluetooth;

namespace MeadowApp
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        //==== peripherals
        //RgbPwmLed onboardLed;

        //==== internals
        Definition bleTreeDefinition;
        CharacteristicBool onOffCharacteristic;

        public MeadowApp()
        {
            Initialize();
        }

        void Initialize()
        {
            Console.WriteLine("Initialize hardware...");

            //onboardLed = new RgbPwmLed(device: Device,
            //    redPwmPin: Device.Pins.OnboardLedRed,
            //    greenPwmPin: Device.Pins.OnboardLedGreen,
            //    bluePwmPin: Device.Pins.OnboardLedBlue,
            //    3.3f, 3.3f, 3.3f,
            //    Meadow.Peripherals.Leds.IRgbLed.CommonType.CommonAnode);

            //==== configure bluetooth
            // init coprocessor (provides bluetooth and WiFi)
            Console.WriteLine("Initializing the coprocessor.");
            Device.InitCoprocessor();

            // initialize the bluetooth defnition tree
            Console.WriteLine("Starting the BLE server.");
            bleTreeDefinition = GetDefinition();
            Device.BluetoothAdapter.StartBluetoothStack(bleTreeDefinition);

            // wire up some notifications on set
            foreach (var characteristic in bleTreeDefinition.Services[0].Characteristics) {
                characteristic.ValueSet += (c, d) => {
                    Console.WriteLine($"HEY, I JUST GOT THIS BLE DATA for Characteristic '{c.Name}' of type {d.GetType().Name}: {d}");
                };
            }

            // addressing individual characteristics:
            onOffCharacteristic.ValueSet += (c,d) => {
                Console.WriteLine($"{ c.Name }: {d}");
            };

            Console.WriteLine("Hardware initialized.");
        }

        protected Definition GetDefinition()
        {
            onOffCharacteristic = new CharacteristicBool(
                    "On_Off",
                    Guid.NewGuid().ToString(),
                    CharacteristicPermission.Read | CharacteristicPermission.Write,
                    CharacteristicProperty.Read | CharacteristicProperty.Write);

            var definition = new Definition(
                "MY MEADOW F7",
                new Service(
                    "ServiceA",
                    253,
                    onOffCharacteristic,
                    new CharacteristicBool(
                        "My Bool",
                        uuid: "017e99d6-8a61-11eb-8dcd-0242ac1300aa",
                        permissions: CharacteristicPermission.Read,
                        properties: CharacteristicProperty.Read
                        ),
                    new CharacteristicInt32(
                        "My Number",
                        uuid: "017e99d6-8a61-11eb-8dcd-0242ac1300bb",
                        permissions: CharacteristicPermission.Write | CharacteristicPermission.Read,
                        properties: CharacteristicProperty.Write | CharacteristicProperty.Read
                        ),
                    new CharacteristicString(
                        "My Text",
                        uuid: "017e99d6-8a61-11eb-8dcd-0242ac1300cc",
                        maxLength: 20,
                        permissions: CharacteristicPermission.Write | CharacteristicPermission.Read,
                        properties: CharacteristicProperty.Write | CharacteristicProperty.Read
                        )
                    )
                );
            return definition;
        }
    }
}