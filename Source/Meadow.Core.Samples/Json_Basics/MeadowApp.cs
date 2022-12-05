using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Leds;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Json_Basics
{
    public class Account
    {
        public string Email { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedDate { get; set; }
        public IList<string> Roles { get; set; }
    }

    public class MeadowApp : App<F7FeatherV2>
    {
        RgbPwmLed onboardLed;

        public override Task Initialize()
        {
            onboardLed = new RgbPwmLed(
                device: Device,
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue);

            return base.Initialize();
        }

        public override Task Run()
        {
            Console.WriteLine("Hello Meadow Json serialize");

            onboardLed.SetColor(Color.Orange);

            TestJsonSerialize();

            onboardLed.SetColor(Color.Yellow);

            TestJsonDeserialize();

            onboardLed.SetColor(Color.Green);

            return base.Run();
        }

        void TestJsonSerialize()
        {
            var account = new Account
            {
                Email = "james@example.com",
                Active = true,
                CreatedDate = new DateTime(2013, 1, 20, 0, 0, 0, DateTimeKind.Utc),
                Roles = new List<string>
                {
                    "User",
                    "Admin"
                }
            };

            string json = System.Text.Json.JsonSerializer.Serialize(account);

            Console.WriteLine($"Serialize:\r\n    {json}");
        }

        void TestJsonDeserialize()
        {
            string json = @"{
              ""Email"": ""james@example.com"",
              ""Active"": true,
              ""CreatedDate"": ""2013-01-20T00:00:00Z"",
              ""Roles"": [
                ""User"",
                ""Admin""
              ]
            }";

            Account account = System.Text.Json.JsonSerializer.Deserialize<Account>(json);

            Console.WriteLine($"Deserialize:\r\n" +
                $"    Email: {account.Email}\r\n" +
                $"    Active: {account.Active}\r\n" +
                $"    Created date: {account.CreatedDate}\r\n" +
                $"    Roles[0]: {account.Roles[0]}\r\n" +
                $"    Roles[1]: {account.Roles[1]}");
        }
    }
}