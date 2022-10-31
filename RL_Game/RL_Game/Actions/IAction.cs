namespace RL_Game.Actions
{
    public interface IAction
    {
        int EntityId { get; }

        void PerformAction();
    }
}
