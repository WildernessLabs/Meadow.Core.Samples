using Meadow.Foundation.Audio;
using Meadow.Foundation.Graphics;

namespace Juego.Games
{
    public interface IGame
    {
        void Left();
        void Right();
        void Up();
        void Down();

        void Update(IIOConfig ioConfig);

        void Init(MicroGraphics gl);

        void Reset();
    }
}