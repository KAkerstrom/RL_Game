using RL_Game.Core;
using RogueSharp;

namespace RL_Game.Components
{
    public class PositionComponent : Component
    {
        public int X;
        public int Y;

        public Point Point => new Point(X, Y);

        public PositionComponent(int x, int y) : base((int)ComponentTypeIds.Position)
        {
            X = x;
            Y = y;
        }

        public PositionComponent(Point point) : base((int)ComponentTypeIds.Position)
        {
            X = point.X;
            Y = point.Y;
        }
    }
}
