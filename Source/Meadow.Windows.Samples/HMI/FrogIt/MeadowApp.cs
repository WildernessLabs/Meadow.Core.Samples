using Juego.Games;
using Meadow;
using Meadow.Foundation.Audio;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Foundation.Sensors.Hid;
using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Juego
{
    public class WindowsHardware : IIOConfig
    {
        public MicroGraphics Graphics { get; protected set; }

        public AnalogJoystick Joystick { get; protected set; }

        public PushButton Up { get; protected set; }
        public PushButton Down { get; protected set; }
        public PushButton Left { get; protected set; }
        public PushButton Right { get; protected set; }
        public PushButton Start { get; protected set; }
        public PushButton Select { get; protected set; }

        public PiezoSpeaker speakerLeft { get; protected set; }
        public PiezoSpeaker speakerRight { get; protected set; }
        public RgbPwmLed rgbLed { get; protected set; }

        public WindowsHardware(WinFormsDisplay display)
        {
            var keyboard = new Keyboard();

            Graphics = new MicroGraphics(display);
            /*
            {
                CurrentFont = new Font12x20(),
                Rotation = RotationType._180Degrees,
                IgnoreOutOfBoundsPixels = true,
            };
            */
            Up = new PushButton(keyboard.Pins.Up);
            Down = new PushButton(keyboard.Pins.Down);
            Left = new PushButton(keyboard.Pins.Left);
            Right = new PushButton(keyboard.Pins.Right);
            Start = new PushButton(keyboard.Pins.Num1);
            Select = new PushButton(keyboard.Pins.Num2);
        }
    }

    public class MeadowApp : App<Windows>
    {
        public static async Task Main(string[] args)
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            ApplicationConfiguration.Initialize();

            await MeadowOS.Start(args);
        }

        bool playGame = false;
        IIOConfig hardware;
        IGame currentGame;
        WinFormsDisplay display;

        public override Task Initialize()
        {
            Resolver.Log.Info("Initialize game hardware...");

            display = new WinFormsDisplay(200, 200);

            hardware = new WindowsHardware(display);

            hardware.Up.Clicked += Up_Clicked;
            hardware.Left.Clicked += Left_Clicked;
            hardware.Right.Clicked += Right_Clicked;
            hardware.Down.Clicked += Down_Clicked;

            return base.Initialize();
        }

        public override Task Run()
        {
            _ = Task.Run(async () =>
            {
                await Task.Delay(1000);
                await StartGame();
            });

            System.Windows.Forms.Application.Run(display);
            return base.Run();
        }

        async Task StartGame()
        {
            currentGame = new FrogItGame();

            playGame = true;
            currentGame.Init(hardware.Graphics);
            currentGame.Reset();

            while (playGame == true)
            {
                display.Invoke(() =>
                {
                    currentGame.Update(hardware);
                });

                Thread.Sleep(1);
            }
        }

        private void Down_Clicked(object sender, EventArgs e)
        {
            currentGame?.Down();
        }

        private void Right_Clicked(object sender, EventArgs e)
        {
            currentGame?.Right();
        }

        private void Left_Clicked(object sender, EventArgs e)
        {
            currentGame?.Left();
        }

        private void Up_Clicked(object sender, EventArgs e)
        {
            currentGame?.Up();
        }

        private void Select_Clicked(object sender, EventArgs e)
        {
            playGame = false;

        }

        private void Start_Clicked(object sender, EventArgs e)
        {
        }

        byte[] LoadResource(string filename)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"Juego.{filename}";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (var ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    return ms.ToArray();
                }
            }
        }
    }
}