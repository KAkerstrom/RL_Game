using RLNET;
using RogueSharp;

namespace RL_Game.Core
{
    public abstract class Actor
    {
        public int Sight { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public ColorSet Colors { get; set; }
        public char Symbol { get; set; }

        public void Draw(RLConsole console, IMap map)
        {
            // Don't draw actors in cells that haven't been explored
            if (!map.GetCell(X, Y).IsExplored)
            {
                return;
            }

            // Only draw the actor with the color and symbol when they are in field-of-view
            if (map.IsInFov(X, Y))
            {
                console.Set(X, Y, Colors.ForegroundVisible, DefaultColors.BackgroundVisible, Symbol);
            }
            else
            {
                // When not in field-of-view just draw a normal floor
                console.Set(X, Y, DefaultColors.BackgroundVisible, DefaultColors.BackgroundVisible, '.');
            }
        }
    }
}
