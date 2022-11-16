using RogueSharp;
using RogueSharp.MapCreation;
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
            IMapCreationStrategy<Map> mapCreationStrategy = new RandomRoomsMapCreationStrategy<Map>(width, height, 30, 5, 3);
            IMap generated;
            do
            {
                generated = Map.Create(mapCreationStrategy);
            }
            while (!generated.GetCell(5, 5).IsWalkable); // Ensure the player's tile is walkable

            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    tiles[i, j] = generated.GetCell(i, j).IsWalkable
                        ? Tile.Floor
                        : Tile.StoneWall;
                }
            }

            var map = new GameMap(tiles, new RogueSharp.Point(41, 21));
            return map;
        }
    }
}
