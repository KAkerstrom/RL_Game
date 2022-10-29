using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RL_Game.Core
{
    public static class MapFactory
    {
        public static GameMap CreateMap(int width, int height)
        {
            var tiles = new Tile[width, height];
            var player = new Player();
            var map = new GameMap(tiles, new RogueSharp.Point(40, 22), new List<Actor>(), player);
            return map;
        }
    }
}
