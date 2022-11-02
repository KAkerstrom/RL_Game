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

        public MoveAction(int entityId, int xDirection, int yDirection) : base(entityId)
        {
            Direction = new Point(xDirection, yDirection);
        }

        public MoveAction(int entityId, Point direction) : base(entityId)
        {
            Direction = direction;
        }

        protected override void PerformAction(ActionResponse response)
        {
            var entity = EntityManager.GetEntity(EntityId);
            var positionComponent = entity?.GetFirstComponent((int)Component.ComponentTypeIds.Position) as PositionComponent;
            if(positionComponent == null) return;

            positionComponent.X += Direction.X;
            positionComponent.Y += Direction.Y;
        }
    }
}
