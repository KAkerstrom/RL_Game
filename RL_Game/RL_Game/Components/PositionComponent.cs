using RL_Game.Core;
using RogueSharp;

namespace RL_Game.Components
{
    public class PositionComponent : Component
    {
        public int X;
        public int Y;

        public Point Point => new Point(X, Y);

        private bool isTrigger=false;
        public PositionComponent(int x, int y, bool IsTrigger) : base((int)ComponentTypeIds.Position)
        {
            X = x;
            Y = y;
            this.isTrigger = IsTrigger;
        }

        public PositionComponent(Point point,bool isTrigger) : base((int)ComponentTypeIds.Position)
        {
            X = point.X;
            Y = point.Y;
            this.isTrigger=IsTrigger;   
        }
        
        public bool IsTrigger
        {
            get { return isTrigger; }
        }
    }
}
