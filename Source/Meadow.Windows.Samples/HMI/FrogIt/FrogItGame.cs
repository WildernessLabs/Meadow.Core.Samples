using Meadow;
using System;
using System.Diagnostics;

namespace Juego.Games
{
    public partial class FrogItGame : IGame
    {
        enum FrogState
        {
            Forward,
            Left,
            Right,
            Dead
        }

        FrogState frogState;

        //each lane has a velocity
        public double[] LaneSpeeds { get; private set; } = new double[6] { 1.0f, -2.0f, 1.5f, -1.0f, 1.5f, -2.0f };
        public byte[,] LaneData { get; private set; } = new byte[6, 32]
        {
            //no data for docks
            {1,2,3,0,1,2,3,0,0,0,1,2,3,0,1,3,0,0,0,0,1,2,3,0,0,0,0,1,2,3,0,0 },//logs
            {0,0,1,3,0,0,0,1,3,0,0,0,1,3,0,0,1,2,3,0,0,0,0,0,1,3,0,0,1,3,0,0 },//logs
            {1,2,3,0,1,2,3,0,0,0,1,2,3,0,1,2,3,0,0,0,1,2,2,3,0,0,0,1,2,3,0,0 },//logs
            {0,0,1,3,0,1,3,0,0,0,0,0,0,0,0,0,1,3,0,0,0,0,0,0,1,3,0,0,1,3,0,0 },//trucks
            {0,0,1,2,0,0,0,0,0,0,0,1,2,0,0,0,1,2,0,0,0,1,2,0,1,2,0,0,0,0,0,0 },//cars
            {1,2,3,0,0,0,0,0,0,0,0,1,2,3,0,0,0,0,0,1,2,3,0,0,0,0,0,1,2,3,0,0 },//trucks
            //no data for start lane
        };

        public double GameTime { get; private set; }
        public double TimeDelta => GameTime - lastTime;

        public int LaneLength => 32;
        public int Columns { get; private set; } = 16;
        public int Rows => 8;

        public double FrogX { get; set; }
        public double FrogY { get; private set; }

        public int Lives { get; private set; }
        public int FrogsHome { get; private set; }

        public int CellSize { get; private set; }

        DateTime gameStart;
        UserInput lastInput;

        enum UserInput
        {
            None,
            Up,
            Down,
            Left,
            Right,
        }

        public FrogItGame(int cellSize = 8, int width = 128)
        {
            CellSize = cellSize;
            Columns = width / cellSize;
            Reset();
        }

        public void Reset()
        {
            gameStart = DateTime.Now;
            ResetFrog();
            Lives = 3;

            FrogsHome = 0;

            lastInput = UserInput.None;
        }

        void ResetFrog()
        {
            FrogX = Columns * CellSize / 2;
            FrogY = (Rows - 1) * CellSize;
            frogState = FrogState.Forward;
        }

        double lastTime;
        int count = 0;
        Stopwatch sw = new Stopwatch();
        public void Update()
        {
            if (count == 0)
            {
                sw.Start();
            }
            else if (count == 100)
            {
                sw.Stop();
                Resolver.Log.Info($"100 frames took {sw.Elapsed}");
                Resolver.Log.Info($"FPS: {100 / sw.Elapsed.TotalSeconds}");
            }

            count++;

            lastTime = GameTime;
            GameTime = (DateTime.Now - gameStart).TotalSeconds;

            switch (lastInput)
            {
                case UserInput.Up:
                    MoveFrogUp();
                    break;
                case UserInput.Down:
                    MoveFrogDown();
                    break;
                case UserInput.Left:
                    MoveFrogLeft();
                    break;
                case UserInput.Right:
                    MoveFrogRight();
                    break;
            }
            //clear for next frame
            lastInput = UserInput.None;
        }

        public void Up()
        {
            lastInput = UserInput.Up;
        }

        public void Down()
        {
            lastInput = UserInput.Down;
        }

        public void Left()
        {
            lastInput = UserInput.Left;
        }

        public void Right()
        {
            lastInput = UserInput.Right;
        }

        void MoveFrogUp()
        {
            frogState = FrogState.Forward;
            if (FrogY >= CellSize) { FrogY -= CellSize; }

            if (FrogY == 0)
            {
                FrogsHome++;
                if (FrogsHome >= 5) { Reset(); }
                else { ResetFrog(); }
            }
        }

        void MoveFrogDown()
        {
            frogState = FrogState.Forward;
            if (FrogY < Rows * CellSize - CellSize) { FrogY += CellSize; }
        }

        void MoveFrogLeft()
        {
            frogState = FrogState.Left;
            if (FrogX > CellSize) { FrogX -= CellSize; }
        }

        void MoveFrogRight()
        {
            frogState = FrogState.Right;
            if (FrogX <= Columns * CellSize - CellSize) { FrogX += CellSize; }
        }

        void KillFrog()
        {
            frogState = FrogState.Dead;
            ResetFrog();
        }
    }
}