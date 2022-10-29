using RLNET;
using RL_Game.Core;
using System;
using RogueSharp.Random;
using System.Numerics;

namespace RL_Game
{
    public static class Game
    {
        // The screen height and width are in number of tiles
        private static readonly int _screenWidth = 200;
        private static readonly int _screenHeight = 100;
        private static RLRootConsole _rootConsole;


        // The map console takes up most of the screen and is where the map will be drawn
        private static readonly int _mapWidth = 180;
        private static readonly int _mapHeight = 75;
        private static RLConsole _mapConsole;

        // Below the map console is the message console which displays attack rolls and other information
        private static readonly int _messageWidth = 180;
        private static readonly int _messageHeight = 12;
        private static RLConsole _messageConsole;

        // The stat console is to the right of the map and display player and monster stats
        private static readonly int _statWidth = 20;
        private static readonly int _statHeight = 100;
        private static RLConsole _statConsole;

        // Above the map is the inventory console which shows the players equipment, abilities, and items
        private static readonly int _inventoryWidth = 180;
        private static readonly int _inventoryHeight = 11;
        private static RLConsole _inventoryConsole;

        public static GameMap GameMap { get; private set; }
        public static IRandom Random { get; private set; } // For generating the level seed

        public static Player Player { get; set; }

        private static bool _renderRequired = true;



        public static void Main()
        {
            // Establish the seed for the random number generator from the current time
            int seed = (int)DateTime.UtcNow.Ticks;
            Random = new DotNetRandom(seed);

            // This must be the exact name of the bitmap font file we are using or it will error.
            string fontFileName = "terminal8x8.png";

            // The title will appear at the top of the console window along with the seed used to generate the level
            string consoleTitle = $"RL Game";

            // Tell RLNet to use the bitmap font that we specified and that each tile is 8 x 8 pixels
            _rootConsole = new RLRootConsole(fontFileName, _screenWidth, _screenHeight, 8, 8, 1f, consoleTitle);

            // Initialize the sub consoles that we will Blit to the root console
            _mapConsole = new RLConsole(_mapWidth, _mapHeight);
            _messageConsole = new RLConsole(_messageWidth, _messageHeight);
            _statConsole = new RLConsole(_statWidth, _statHeight);
            _inventoryConsole = new RLConsole(_inventoryWidth, _inventoryHeight);

            //MapGenerator mapGenerator = new MapGenerator(_mapWidth, _mapHeight, 40, 22, 7, _mapLevel);
            GameMap = MapFactory.CreateMap(200, 200);
            GameMap.UpdatePlayerFieldOfView();

            CommandSystem = new CommandSystem();

            // Set up a handler for RLNET's Update event
            _rootConsole.Update += OnRootConsoleUpdate;

            // Set up a handler for RLNET's Render event
            _rootConsole.Render += OnRootConsoleRender;

            //_inventoryConsole.SetBackColor(0, 0, _inventoryWidth, _inventoryHeight, Swatch.DbWood);
            // _inventoryConsole.Print(1, 1, "Inventory", Colors.TextHeading);

            // Begin RLNET's game loop
            _rootConsole.Run();

        }

        // Event handler for RLNET's Update event
        private static void OnRootConsoleUpdate(object sender, UpdateEventArgs e)
        {
            bool didPlayerAct = false;
            RLKeyPress keyPress = _rootConsole.Keyboard.GetKeyPress();

            // Handles player movement and turns by characters
            if (CommandSystem.IsPlayerTurn)
            {
                if (keyPress != null)
                {
                    if (keyPress.Key == RLKey.Up)
                    {
                        didPlayerAct = CommandSystem.MovePlayer(Direction.Up);
                    }
                    else if (keyPress.Key == RLKey.Down)
                    {
                        didPlayerAct = CommandSystem.MovePlayer(Direction.Down);
                    }
                    else if (keyPress.Key == RLKey.Left)
                    {
                        didPlayerAct = CommandSystem.MovePlayer(Direction.Left);
                    }
                    else if (keyPress.Key == RLKey.Right)
                    {
                        didPlayerAct = CommandSystem.MovePlayer(Direction.Right);
                    }
                    else if (keyPress.Key == RLKey.Escape)
                    {
                        _rootConsole.Close();
                    }
                    else if (keyPress.Key == RLKey.Space)
                    {
                        if (GameMap.CanMoveDownToNextLevel())
                        {
                            MapGenerator mapGenerator = new MapGenerator(_mapWidth, _mapHeight, 20, 13, 7, ++_mapLevel);
                            GameMap = mapGenerator.CreateMap();
                            MessageLog = new MessageLog();
                            CommandSystem = new CommandSystem();
                            _rootConsole.Title = $"Rogue - Level {_mapLevel}";
                            didPlayerAct = true;
                        }
                    }
                }

                if (didPlayerAct)
                {
                    _renderRequired = true;
                    CommandSystem.EndPlayerTurn();
                }
            }
            else
            {
                CommandSystem.ActivateMonsters();
                _renderRequired = true;
            }
        }

        // Event handler for RLNET's Render event
        private static void OnRootConsoleRender(object sender, UpdateEventArgs e)
        {
            if (_renderRequired)
            {
                // Clear the consoles for a new level
                _mapConsole.Clear();
                _statConsole.Clear();
                _messageConsole.Clear();
                _inventoryConsole.Clear();

                // Draw everything to the map
                GameMap.Draw(_mapConsole, _statConsole);
                Player.Draw(_mapConsole, GameMap);
                Player.DrawStats(_statConsole, _statWidth, _statHeight);
                Player.DrawInventory(_inventoryConsole);
                MessageLog.Draw(_messageConsole, _messageWidth, _messageHeight);

                // Blit the sub consoles to the root console in the correct locations
                RLConsole.Blit(_mapConsole, 0, 0, _mapWidth, _mapHeight, _rootConsole, 0, _inventoryHeight);
                RLConsole.Blit(_messageConsole, 0, 0, _messageWidth, _messageHeight, _rootConsole, 0, _screenHeight - _messageHeight);
                RLConsole.Blit(_statConsole, 0, 0, _statWidth, _statHeight, _rootConsole, _mapWidth, 0);
                RLConsole.Blit(_inventoryConsole, 0, 0, _inventoryWidth, _inventoryHeight, _rootConsole, 0, 0);

                // Tell RLNET to draw the console that we set
                _rootConsole.Draw();

                _renderRequired = false;
            }
        }

    }
}