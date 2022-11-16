namespace RL_Game.Actions
{
    public abstract class Action
    {
        public enum ActionResponse // TODO - This seems like it'll need something more descriptive rather than just a boolean
        {
            Allow,
            Block
        }

        public enum ActionTypes
        {
            Move,
        }

        public delegate ActionResponse BeforeActionPerformedDelegate(Action action);
        public static BeforeActionPerformedDelegate BeforeActionPerformed;

        public delegate void AfterActionPerformedDelegate(Action action);
        public static AfterActionPerformedDelegate AfterActionPerformed;

        public int EntityId { get; protected set; }

        public ActionTypes ActionType;

        public Action(int entityId, ActionTypes actionType)
        {
            EntityId = entityId;
            ActionType = actionType;
        }

        public void Perform()
        {
            // Check if anything interferes with this action being performed
            var allResponses = BeforeActionPerformed
                ?.GetInvocationList()
                .Select(x => ((BeforeActionPerformedDelegate)x)(this));
            var response = allResponses != null && allResponses.Any(value => value == ActionResponse.Block)
                ? ActionResponse.Block
                : ActionResponse.Allow;

            PerformAction(response);

            AfterActionPerformed?.Invoke(this);
        }

        protected abstract void PerformAction(ActionResponse response);
    }
}
