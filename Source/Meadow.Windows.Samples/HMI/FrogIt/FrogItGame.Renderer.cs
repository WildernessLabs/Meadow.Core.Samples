using System;
using Meadow.Foundation.Audio;
using Meadow.Foundation.Graphics;

namespace Juego.Games
{
    public partial class FrogItGame
    {
        readonly byte cellSize = 8;

        DrawPixelDel DrawPixel;

        public void Init(MicroGraphics gl)
        {
            InitBuffers();

            gl.Clear();
            gl.DrawText(0, 0, "Meadow FrogIt");
            gl.DrawText(0, 16, "v0.3.0");
            gl.Show();

            //hacky scale for now
            if (gl.Width >= 160 && gl.Height >= 128)
            {
                DrawPixel = DrawPixel2x;
            }
            else
            {
                DrawPixel = DrawPixel1x;
            }
        }

        public void Update(IIOConfig ioConfig)
        {
            var gl = ioConfig.Graphics;

            Update();

            gl.Clear();
            DrawBackground(gl);
            DrawLanesAndCheckCollisions(gl);
            DrawFrog(gl, frogState);
            // DrawLives();
            gl.Show();
        }

        void DrawBackground(MicroGraphics graphics)
        {
            //draw docks
            for (int i = 0; i < 5; i++)
            {
                //  graphics.DrawRectangle(10 + 24 * i, 0, 12, 8, true, false);

                if (i < FrogsHome)
                {
                    DrawFrog(12 + 24 * i, 0, FrogState.Forward, graphics);
                }
            }

            //draw water
            //graphics.DrawRectangle(0, cellSize, 128, cellSize * 3, true, true);
        }

        void DrawLanesAndCheckCollisions(MicroGraphics graphics)
        {
            int startPos, index, x, y;
            int cellOffset;

            double offsetD;

            for (byte row = 0; row < 6; row++)
            {
                startPos = (int)(GameTime * LaneSpeeds[row]) % LaneLength;
                offsetD = 8.0 * GameTime * LaneSpeeds[row];

                cellOffset = ((int)(offsetD)) % cellSize;

                if (startPos < 0)
                {
                    startPos = LaneLength - ((0 - startPos)%32);//     (Math.Abs(startPos) % 32);
                }

                y = cellSize * (row + 1);

                if (row < 3 && y == FrogY)
                {
                    FrogX -= TimeDelta * LaneSpeeds[row] * 8;
                }

                for (byte i = 0; i < Columns + 2; i++)
                {
                    index = LaneData[row, (startPos + i) % LaneLength];

                    x = (i - 1) * cellSize - cellOffset;

                    if (index == 0)
                    {
                        if (row < 3)
                        {
                            if (IsFrogCollision(x, y) == true)
                            {
                                KillFrog();
                            }
                        }
                        continue;
                    }

                    if (x < 0 || x >= graphics.Width - 8)
                        continue;

                    switch (row)
                    {
                        case 0:
                        case 1:
                        case 2:
                            DrawLog(x, y, index, graphics);
                            break;
                        case 3:
                        case 5:
                            DrawTruck(x, y, index, graphics);
                            if (IsFrogCollision(x, y)) { KillFrog(); }
                            break;
                        case 4:
                            DrawCar(x, y, index, graphics);
                            if (IsFrogCollision(x, y)) { KillFrog(); }
                            break;
                    }
                }
            }
        }

        bool IsFrogCollision(int x, int y)
        {
            if (y == FrogY &&
                x > FrogX &&
                x < FrogX + cellSize)
            {
                return true;
            }
            return false;
        }

        void DrawLives(MicroGraphics graphics)
        {
            for (int i = 1; i < Lives; i++)
            {
                DrawFrog(cellSize * (Columns - i), cellSize * (Rows - 1), FrogState.Forward, graphics);
            }
        }

        void DrawFrog(MicroGraphics graphics, FrogState state = FrogState.Forward)
        {
            DrawFrog((int)Math.Truncate(FrogX), (int)Math.Truncate(FrogY), state, graphics);
        }

        void DrawFrog(int x, int y, FrogState state, MicroGraphics graphics)
        {
            if (state == FrogState.Left)
            {
                graphics.DrawBuffer(x, y, frogLeft);   
            }
            else if (state == FrogState.Forward)
            {
                graphics.DrawBuffer(x, y, frogUp);
            }
            else if (state == FrogState.Right)
            {
                graphics.DrawBuffer(x, y, frogRight);
            }
            else
            {
                graphics.DrawText(x, y, "X");
            }
        }

        void DrawTruck(int x, int y, int index, MicroGraphics graphics)
        {
            if (index == 1) graphics.DrawBuffer(x, y, truckLeft);
            else if (index == 2) graphics.DrawBuffer(x, y, truckCenter);
            else if (index == 3) graphics.DrawBuffer(x, y, truckRight);
        }

        void DrawLog(int x, int y, int index, MicroGraphics graphics)
        {
            if (index == 1) graphics.DrawBuffer(x, y, logDarkLeft);
            else if (index == 2) graphics.DrawBuffer(x, y, logDarkCenter);
            else if (index == 3) graphics.DrawBuffer(x, y, logDarkRight);
        }

        void DrawCar(int x, int y, int index, MicroGraphics graphics)
        {
            if (index == 1) graphics.DrawBuffer(x, y, carLeft);
            else if (index == 2) graphics.DrawBuffer(x, y, carRight);
        }

        delegate void DrawPixelDel(int x, int y, bool colored, MicroGraphics graphics);

        void DrawPixel1x(int x, int y, bool colored, MicroGraphics graphics)
        {
            graphics.DrawPixel(x, y, colored);
        }

        void DrawPixel2x(int x, int y, bool colored, MicroGraphics graphics)
        {
            x *= 2;
            y *= 2;

            graphics.DrawPixel(x, y, colored);
            graphics.DrawPixel(x + 1, y, colored);
            graphics.DrawPixel(x, y + 1, colored);
            graphics.DrawPixel(x + 1, y + 1, colored);
        }

        /*
        void DrawBitmap(int x, int y, int width, int height, byte[] bitmap, GraphicsLibrary graphics)
        {
            for (var ordinate = 0; ordinate < height; ordinate++) //y
            {
                for (var abscissa = 0; abscissa < width; abscissa++) //x
                {
                    var b = bitmap[(ordinate * width) + abscissa];
                    byte mask = 0x01;

                    for (var pixel = 0; pixel < 8; pixel++)
                    {
                        DrawPixel(x + (8 * abscissa) + 7 - pixel, y + ordinate, (b & mask) > 0, graphics);

                        mask <<= 1;
                    }
                }
            }
        }*/
    }
}