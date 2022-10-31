using RL_Game.Actions;

namespace RL_Game.Systems
{
    public static class ActionSystem
    {
        private static Queue<IAction> _actionQueue = new Queue<IAction>();
        private static Queue<IAction> ActionQueue => _actionQueue;

        /// <summary>
        /// Returns whether there are actions queued.
        /// </summary>
        public static bool AreActionsQueued => ActionQueue.Count > 0;

        /// <summary>
        /// Add an action to the queue.
        /// </summary>
        /// <param name="action">The action to queue.</param>
        public static void EnqueueAction(IAction action)
        {
            ActionQueue.Enqueue(action);
        }

        /// <summary>
        /// Gets the next action from the queue.
        /// </summary>
        /// <returns></returns>
        public static IAction GetNextAction()
        {
            return ActionQueue.Dequeue();
        }

        public static IEnumerable<IAction> DequeueAllActions()
        {
            while(ActionQueue.Count() > 0)
            {
                yield return ActionQueue.Dequeue();
            }
        }
    }
}
