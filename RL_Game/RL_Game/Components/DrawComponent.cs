using RL_Game.Core;
using RLNET;

namespace RL_Game.Components
{
    internal class DrawComponent : Component
    {
        public char Symbol;
        public RLColor Forecolor;

        public DrawComponent(char symbol, RLColor color) : base((int)ComponentTypeIds.Draw) {
            Symbol = symbol;
            Forecolor = color;
        }
    }
}
