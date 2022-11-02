using RL_Game.Components;
using RLNET;
using RogueSharp;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace RL_Game.Core
{
    public class GameMap : Map
    {
        private Rectangle _playerView = new Rectangle();
        private Tile[,] _tiles;
        private FieldOfView _fov;

        public GameMap(Tile[,] tileMap, Point playerViewSize)
            : base(tileMap.GetUpperBound(0) + 1, tileMap.GetUpperBound(1) + 1)
        {
            var player = EntityManager.PlayerEntity;
            var playerPositionComponent = player.GetFirstComponent((int)Component.ComponentTypeIds.Position) as PositionComponent;
            if(playerPositionComponent == null)
            {
                throw new NullReferenceException("Player missing position component.");
            }
            var playerPosition = playerPositionComponent.Point;

            _tiles = tileMap;
            _fov = new FieldOfView(this);

            _playerView = new Rectangle(playerPosition.X - playerViewSize.X / 2, playerPosition.Y - playerViewSize.Y / 2, playerViewSize.X, playerViewSize.Y);
            if(_playerView.Top < 0)
            {
                _playerView.Offset(0, _playerView.Top * -1);
            }
            if (_playerView.Left < 0)
            {
                _playerView.Offset(_playerView.Left * -1, 0);
            }  
        }

        public void Draw(RLConsole mapConsole, RLConsole statConsole)
        {
            foreach (var cell in GetCellsInRectangle(_playerView))
            {
                if (cell.IsExplored)
                {
                    if (_fov.IsInFov(cell.X, cell.Y))
                    {
                        if (cell.IsWalkable)
                        {
                            mapConsole.Set(cell.X, cell.Y, DefaultColors.BackgroundVisible, DefaultColors.BackgroundVisible, ' ');
                        }
                        else
                        {
                            mapConsole.Set(cell.X, cell.Y, DefaultColors.ForegroundVisible, DefaultColors.BackgroundVisible, '#');
                        }

                    }
                }
            }

            foreach (var entity in GetEntitiesInFov(_fov, EntityManager.PlayerEntity))
            {
                var draw = entity.GetFirstComponent((int)Component.ComponentTypeIds.Draw) as DrawComponent;
                var position = entity.GetFirstComponent((int)Component.ComponentTypeIds.Position) as PositionComponent;

                if (draw == null || position == null)
                {
                    continue;
                }
                mapConsole.Set(position.X - _playerView.Left, position.Y - _playerView.Top, draw.Forecolor, DefaultColors.BackgroundVisible, draw.Symbol);
            }
        }

        public void UpdatePlayerFov()
        {
            var player = EntityManager.PlayerEntity;
            var playerPositionComponent = player.GetFirstComponent((int)Component.ComponentTypeIds.Position) as PositionComponent;
            if (playerPositionComponent == null)
            {
                throw new NullReferenceException("Player missing position component.");
            }
            var playerPosition = playerPositionComponent.Point;

            var cellsInView = _fov.ComputeFov(playerPosition.X, playerPosition.Y, 5, true); // TODO - variable sight
            foreach (Cell cell in cellsInView)
            {
                SetCellProperties(cell.X, cell.Y, cell.IsTransparent, cell.IsWalkable, true);
            }
        }

        private IEnumerable<ICell> GetCellsInRectangle(Rectangle rectangle)
        {
            for (int x = rectangle.Left; x < rectangle.Left + rectangle.Width; x++)
            {
                for (int y = rectangle.Top; y < rectangle.Top + rectangle.Height; y++)
                {
                    if (x < 0 || x > Width || y < 0 || y > Height)
                    {
                        continue;
                    }

                    yield return GetCell(x, y);
                }
            }
        }

        private IEnumerable<Entity> GetEntitiesInFov(FieldOfView fov, Entity fovEntity)
        {
            var fovPosition = fovEntity.GetFirstComponent((int)Component.ComponentTypeIds.Position) as PositionComponent;
            if(fovPosition == null)
            {
                yield break;
            }

            var cellsInView = fov.ComputeFov(fovPosition.X, fovPosition.Y, 5, true);

            foreach (var entity in EntityManager.GetEntitiesWithComponentType((int)Component.ComponentTypeIds.Draw))
            {
                var draw = entity.GetFirstComponent((int)Component.ComponentTypeIds.Draw) as DrawComponent;
                var position = entity.GetFirstComponent((int)Component.ComponentTypeIds.Position) as PositionComponent;

                if(draw == null || position == null)
                {
                    continue;
                }

                if(fov.IsInFov(position.X, position.Y))
                {
                    yield return entity;
                }
            }
        }
    }
}