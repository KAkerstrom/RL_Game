using RL_Game.Components;
using RLNET;
using RogueSharp;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace RL_Game.Core
{
    public class GameMap : Map
    {
        public delegate void NewLevel();      
        private Rectangle _playerView = new Rectangle();
        private Tile[,] _tiles;
        private FieldOfView _fov;
        private Entity _endgoal;
        public GameMap(Tile[,] tileMap, Point playerViewSize,Entity endgoal)
        public Rectangle View { get => new Rectangle(0, 0, _playerView.Width, _playerView.Height); }

        public GameMap(Tile[,] tileMap, Point playerViewSize)
            : base(tileMap.GetUpperBound(0) + 1, tileMap.GetUpperBound(1) + 1)
        {
            var player = EntityManager.PlayerEntity;
            var playerPositionComponent = player.GetFirstComponent((int)Component.ComponentTypeIds.Position) as PositionComponent;
            if (playerPositionComponent == null)
            {
                throw new NullReferenceException("Player missing position component.");
            }
            var playerPosition = playerPositionComponent.Point;
            
            _endgoal = endgoal;
            _tiles = tileMap;
            _fov = new FieldOfView(this);

            _playerView = new Rectangle(playerPosition.X - playerViewSize.X / 2, playerPosition.Y - playerViewSize.Y / 2, playerViewSize.X, playerViewSize.Y);
            if (_playerView.Top < 0) _playerView.Offset(0, _playerView.Top * -1);
            if (_playerView.Left < 0) _playerView.Offset(_playerView.Left * -1, 0);
            //if (_playerView.Bottom > Height) _playerView.Offset(0, (_playerView.Bottom - Height) * -1);
            //if (_playerView.Left > Width) _playerView.Offset((_playerView.Right - Width) * -1, 0);


            for (var i = 0; i < _tiles.GetUpperBound(0); i++)
            {
                for (var j = 0; j < _tiles.GetUpperBound(1); j++)
                {
                    var walkable = _tiles[i, j] != Tile.StoneWall;
                    SetCellProperties(i, j, walkable, walkable);
                }
            }

            foreach (var entity in EntityManager.GetEntitiesWithComponentType((int)Component.ComponentTypeIds.Position))
            {
                var position = entity.GetFirstComponent((int)Component.ComponentTypeIds.Position) as PositionComponent;
                if (position == null)
                {
                    continue;
                }
                SetCellProperties(position.X, position.Y, true, position.IsTrigger);
            }
        }

        public void Draw(RLConsole mapConsole, NewLevel nLevel)
        {
            foreach (var cell in GetCellsInRectangle(_playerView))
            {
                if (_fov.IsInFov(cell.X, cell.Y))
                {
                    if (cell.IsWalkable)
                    {
                        //mapConsole.Set(cell.X - _playerView.Left, cell.Y - _playerView.Top, DefaultColors.BackgroundVisible, DefaultColors.BackgroundVisible, ' ');
                    }
                    else
                    {
                        mapConsole.Set(cell.X - _playerView.Left, cell.Y - _playerView.Top, DefaultColors.ForegroundVisible, DefaultColors.BackgroundVisible, '#');
                    }
                }
                else if (cell.IsExplored)
                {
                    if (cell.IsWalkable)
                    {
                        //mapConsole.Set(cell.X - _playerView.Left, cell.Y - _playerView.Top, DefaultColors.BackgroundVisible, DefaultColors.BackgroundVisible, ' ');
                    }
                    else
                    {
                        mapConsole.Set(cell.X - _playerView.Left, cell.Y - _playerView.Top, DefaultColors.ForegroundHidden, DefaultColors.BackgroundVisible, '#');
                    }
                }
            }

            foreach (var entity in GetEntitiesInFov(_fov, EntityManager.PlayerEntity))
            {
                var draw = entity.GetFirstComponent((int)Component.ComponentTypeIds.Draw) as DrawComponent;
                var position = entity.GetFirstComponent((int)Component.ComponentTypeIds.Position) as PositionComponent;
                var endGoalPosition=_endgoal.GetFirstComponent((int)Component.ComponentTypeIds.Position) as PositionComponent;
                bool breakFlag = false;
                if (endGoalPosition == null)
                {
                    throw new NullReferenceException("Staircase is missing position component");
                }
                if (draw == null || position == null)
                {
                    continue;
                }
                //Checks if Player has stepped on staircase and then calls for a new level to be generated.
                if(entity.Tag=="Player" & (endGoalPosition.Point == position.Point))
                {                   
                    EntityManager.RemoveEntity(_endgoal.Id);
                    position.X =5;position.Y =5;
                    nLevel();
                    breakFlag=true;
                }
                mapConsole.Set(position.X - _playerView.Left, position.Y - _playerView.Top, draw.Forecolor, DefaultColors.BackgroundVisible, draw.Symbol);
                if (breakFlag) { break; }
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

        public void SetPlayerViewCenter(Point point)
        {
            _playerView.X = point.X - _playerView.Width / 2;
            _playerView.Y = point.Y - _playerView.Height / 2;
            if (_playerView.Top < 0) _playerView.Y = 0;
            if (_playerView.Left < 0) _playerView.X = 0;
            if (_playerView.Bottom >= Height) _playerView.Y = Height - _playerView.Height;
            if (_playerView.Right >= Width) _playerView.X = Width - _playerView.Width;
            UpdatePlayerFov();
        }

        //public Point WorldPointToViewPoint(Point absolute)
        //{
        //    // Create a version of the player view that isn't adjusted for world bounds
        //}

        private IEnumerable<ICell> GetCellsInRectangle(Rectangle rectangle)
        {
            for (int x = rectangle.Left; x < rectangle.Left + rectangle.Width; x++)
            {
                for (int y = rectangle.Top; y < rectangle.Top + rectangle.Height; y++)
                {
                    if (x < 0 || x >= Width || y < 0 || y >= Height)
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
            if (fovPosition == null)
            {
                yield break;
            }

            var cellsInView = fov.ComputeFov(fovPosition.X, fovPosition.Y, 5, true);

            foreach (var entity in EntityManager.GetEntitiesWithComponentType((int)Component.ComponentTypeIds.Draw))
            {
                var draw = entity.GetFirstComponent((int)Component.ComponentTypeIds.Draw) as DrawComponent;
                var position = entity.GetFirstComponent((int)Component.ComponentTypeIds.Position) as PositionComponent;

                if (draw == null || position == null)
                {
                    continue;
                }

                if (fov.IsInFov(position.X, position.Y))
                {
                    yield return entity;
                }
            }
        }
    }
}