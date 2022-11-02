using RL_Game.Actions;
using Action = RL_Game.Actions.Action;

namespace RL_Game.Systems
{
    public static class ActionSystem
    {
        private static Queue<Action> _actionQueue = new Queue<Action>();
        private static Queue<Action> ActionQueue => _actionQueue;

        /// <summary>
        /// Returns whether there are actions queued.
        /// </summary>
        public static bool AreActionsQueued => ActionQueue.Count > 0;

        /// <summary>
        /// Add an action to the queue.
        /// </summary>
        /// <param name="action">The action to queue.</param>
        public static void EnqueueAction(Action action)
        {
            ActionQueue.Enqueue(action);
        }

        /// <summary>
        /// Gets the next action from the queue.
        /// </summary>
        /// <returns></returns>
        public static Action GetNextAction()
        {
            return ActionQueue.Dequeue();
        }

        public static IEnumerable<Action> DequeueAllActions()
        {
            while(ActionQueue.Count() > 0)
            {
                yield return ActionQueue.Dequeue();
            }
        }
    }
}
