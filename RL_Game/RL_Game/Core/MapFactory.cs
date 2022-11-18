using RL_Game.Components;
using RLNET;
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
            Point goalPoint = new Point(5, 6);
            IMapCreationStrategy<Map> mapCreationStrategy = new RandomRoomsMapCreationStrategy<Map>(width, height, 30, 5, 3);
            IMap generated;
            do
            {
                generated = Map.Create(mapCreationStrategy);
            }
            while (!(generated.GetCell(5, 5).IsWalkable&&generated.GetCell(goalPoint.X,goalPoint.Y).IsWalkable)); // Ensure the player's tile is walkable

            //Temporary stairs addition
            var stairComponents = new List<Component>()
            {
                new DrawComponent('%', RLColor.Magenta),
                new PositionComponent(5,6,true)
            };
            Entity stairs = new Entity(stairComponents);
            stairs.Tag = "Trigger";
            EntityManager.AddEntity(stairs);


            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    tiles[i, j] = generated.GetCell(i, j).IsWalkable
                        ? Tile.Floor
                        : Tile.StoneWall;
                }
            }
            var map = new GameMap(tiles, new RogueSharp.Point(41, 21),stairs);
            return map;
        }
    }
}
