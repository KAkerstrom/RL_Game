using RLNET;
using RogueSharp;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace RL_Game.Core
{
    public class GameMap : Map
    {
        private readonly List<Actor> _nonPlayerActors;
        private Rectangle _playerView = new Rectangle();
        private Tile[,] _tiles;
        private FieldOfView fov;
        private IEnumerable<ICell> _playerFovCells;

        public Player _player { get; }

        public GameMap(Tile[,] tileMap, Point playerViewSize, List<Actor> nonPlayerActors, Player player)
            : base(tileMap.GetUpperBound(0) + 1, tileMap.GetUpperBound(1) + 1)
        {
            _tiles = tileMap;
            _nonPlayerActors = nonPlayerActors;
            _player = player;
            _playerView = new Rectangle(player.X - playerViewSize.X / 2, player.Y - playerViewSize.Y / 2, playerViewSize.X, playerViewSize.Y);
            fov = new FieldOfView(this);
        }

        public void Draw(RLConsole mapConsole, RLConsole statConsole)
        {
            foreach (var cell in GetCellsInRectangle(_playerView))
            {
                if (cell.IsExplored)
                {
                    if (_playerFovCells.Contains(cell))
                    {
                        if (cell.IsWalkable)
                        {
                            mapConsole.Set(cell.X, cell.Y, DefaultColors.BackgroundVisible, DefaultColors.BackgroundVisible, ' ');
                        }
                        else
                        {
                            mapConsole.Set(cell.X, cell.Y, DefaultColors.BackgroundVisible, DefaultColors.ForegroundVisible, '#');
                        }

                    }
                }

            }
        }

        public void UpdatePlayerFov()
        {
            _playerFov = fov.ComputeFov(_player.X, _player.Y, _player.Sight, true);
            foreach (Cell cell in _playerFov)
            {
                SetCellProperties(cell.X, cell.Y, cell.IsTransparent, cell.IsWalkable, true);
            }
        }

        private IEnumerable<ICell> GetCellsInRectangle(Rectangle rectangle)
        {
            for (int x = 0; x < rectangle.Width; x++)
            {
                for (int y = 0; y < rectangle.Height; y++)
                {
                    yield return GetCell(rectangle.Left + x, rectangle.Top + y);
                }
            }
        }
    }
}