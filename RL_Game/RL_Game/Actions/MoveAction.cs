using OpenTK;
using RL_Game.Components;
using RL_Game.Core;
using RogueSharp;

namespace RL_Game.Actions
{
    public class MoveAction : Action
    {
        /// <summary>
        /// The relative direction to move.
        /// </summary>
        public Point Direction { get; }

        public MoveAction(int entityId, int xDirection, int yDirection) : base(entityId, ActionTypes.Move)
        {
            Direction = new Point(xDirection, yDirection);
        }

        public MoveAction(int entityId, Point direction) : base(entityId, ActionTypes.Move)
        {
            Direction = direction;
        }

        protected override void PerformAction(ActionResponse response)
        {
            var entity = EntityManager.GetEntity(EntityId);
            var positionComponent = entity?.GetFirstComponent((int)Component.ComponentTypeIds.Position) as PositionComponent;
            if(positionComponent == null) return;

            var destination = positionComponent.Point + Direction;
            if(destination.X < 0) destination.X = 0;
            if(destination.Y < 0) destination.Y = 0;
            if(destination.Y > Game.GameMap.Height) destination.Y = Game.GameMap.Height;
            if(destination.Y > Game.GameMap.Width) destination.Y = Game.GameMap.Width;
            var currentMapCell = Game.GameMap.GetCell(positionComponent.X, positionComponent.Y);
            var destinationMapCell = Game.GameMap.GetCell(destination.X, destination.Y);

            if (destinationMapCell.IsWalkable)
            {
                // Move the entity
                positionComponent.X = destinationMapCell.X;
                positionComponent.Y = destinationMapCell.Y;
                
                // Update the map's walkability
                Game.GameMap.SetCellProperties(currentMapCell.X, currentMapCell.Y, currentMapCell.IsTransparent, true);
                Game.GameMap.SetCellProperties(destinationMapCell.X, destinationMapCell.Y, destinationMapCell.IsTransparent, false);

                // Handle the player special case
                if(EntityManager.PlayerEntity.Id == entity.Id)
                {
                    Game.GameMap.SetPlayerViewCenter(destination);
                }
            }
            // else? Replace this action with a basic action, depending on the entity at that position?
        }
    }
}
