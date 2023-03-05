using Meadow.Foundation;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Foundation.Sensors.Hid;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Meadow
{
    public class MeadowApp : App<Windows>
    {
        private MicroGraphics _graphics = default!;
        private WinFormsDisplay _display = default!;
        private Keyboard _keyBoard = default!;
        private int _xDirection;
        private int _yDirection;
        private int _speed = 5;
        private int _x;
        private int _y;
        private int _radius = 10;

        public override async Task Run()
        {
            _ = Task.Run(() =>
            {
                Thread.Sleep(1000);
                DisplayLoop();
            });

            Application.Run(_display);
        }

        public override Task Initialize()
        {
            _display = new WinFormsDisplay();
            _graphics = new MicroGraphics(_display);
            _keyBoard = new Keyboard();

            var rightButton = new PushButton(_keyBoard.CreateDigitalInputPort(_keyBoard.Pins.Right));
            var leftButton = new PushButton(_keyBoard.CreateDigitalInputPort(_keyBoard.Pins.Left));
            var upButton = new PushButton(_keyBoard.CreateDigitalInputPort(_keyBoard.Pins.Up));
            var downButton = new PushButton(_keyBoard.CreateDigitalInputPort(_keyBoard.Pins.Down));

            rightButton.PressStarted += (s, e) => { _xDirection = 1; };
            leftButton.PressStarted += (s, e) => { _xDirection = -1; };
            upButton.PressStarted += (s, e) => { _yDirection = -1; };
            downButton.PressStarted += (s, e) => { _yDirection = 1; };

            rightButton.PressEnded += (s, e) => { _xDirection = _yDirection = 0; };
            leftButton.PressEnded += (s, e) => { _xDirection = _yDirection = 0; };
            upButton.PressEnded += (s, e) => { _xDirection = _yDirection = 0; };
            downButton.PressEnded += (s, e) => { _xDirection = _yDirection = 0; };

            _x = _display.Width / 2;
            _y = _display.Height / 2;

            return base.Initialize();
        }

        private void MoveAndDrawCircle()
        {
            var x = _x + (_speed * _xDirection);
            var y = _y + (_speed * _yDirection);

            // check for edge
            if (x - _radius > 0 && x + _radius < _display.Width && x != _x)
            {
                _x = x;
            }
            if (y - _radius > 0 && y + _radius < _display.Height && y != _y)
            {
                _y = y;
            }

            _graphics.DrawCircle(_x, _y, _radius, Color.Yellow, filled: true);
        }

        void DisplayLoop()
        {
            while (true)
            {

                _display.Invoke(() =>
                {
                    // Do your drawing stuff here
                    _graphics.Clear();

                    MoveAndDrawCircle();

                    _graphics.Show();
                });

                Thread.Sleep(33);
            }
        }
    }

}
