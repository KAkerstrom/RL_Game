using RL_Game.Actions;
using RL_Game.Components;
using RL_Game.Core;
using RLNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Action = RL_Game.Actions.Action;

namespace RL_Game.Systems
{
    public static class InputSystem
    {
        public enum Inputs
        {
            Up,
            Down,
            Left,
            Right,
            Select,
            Back,
            Escape
        }

        public static Dictionary<RLKey, Inputs> KeyMap = new Dictionary<RLKey, Inputs>()
        {
            {  RLKey.Up, Inputs.Up },
            {  RLKey.Down, Inputs.Down },
            {  RLKey.Left, Inputs.Left },
            {  RLKey.Right, Inputs.Right },
            {  RLKey.Escape, Inputs.Escape },
        };

        /// <summary>
        /// Process the user's input.
        /// </summary>
        /// <param name="key">The key pressed.</param>
        /// <returns>Whether the input was processed.</returns>
        public static Action? ProcessInput(RLKeyPress key)
        {
            if (key == null || !KeyMap.TryGetValue(key.Key, out var input))
            {
                return null;
            }

            var player = EntityManager.PlayerEntity;
            var playerPositionComponent = player.Components.FirstOrDefault(c => c is PositionComponent);
            if (playerPositionComponent == null)
            {
                throw new NullReferenceException("Player position component is null.");
            }

            switch (input)
            {
                case Inputs.Up:
                    return new MoveAction(player.Id, 0, -1);
                case Inputs.Down:
                    return new MoveAction(player.Id, 0, 1);
                case Inputs.Left:
                    return new MoveAction(player.Id, -1, 0);
                case Inputs.Right:
                    return new MoveAction(player.Id, 1, 0);
                case Inputs.Escape:
                    Game.CloseGame();
                    return null;
            }
            return null;
        }
    }
}
