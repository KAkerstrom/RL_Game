using RL_Game.Components;
using RL_Game.Core;
using RogueSharp;

namespace RL_Game.Actions
{
    public class MoveAction : IAction
    {
        public int EntityId { get; }

        /// <summary>
        /// The relative direction to move.
        /// </summary>
        public Point Direction { get; }

        public MoveAction(int entityId, int xDirection, int yDirection)
        {
            EntityId = entityId;
            Direction = new Point(xDirection, yDirection);
        }

        public MoveAction(int entityId, Point direction)
        {
            EntityId = entityId;
            Direction = direction;
        }

        public void PerformAction()
        {
            var entity = EntityManager.GetEntity(EntityId);
            var positionComponent = entity.GetFirstComponent((int)Component.ComponentTypeIds.Position) as PositionComponent;
            positionComponent.X += Direction.X;
            positionComponent.Y += Direction.Y;
        }
    }
}
