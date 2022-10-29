using RL_Game.Core;
using RLNET;
using RogueSharp;

namespace RL_Game.Interfaces
{
    public interface IDrawable
    {
        public char Symbol { get; }
        public int X { get; }
        public int Y { get; }
        public ColorSet Colors { get; }
        public void Draw(RLConsole console, IMap map);

    }
}
