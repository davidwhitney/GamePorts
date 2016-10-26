namespace ConsoleApplication1.GameModel.Actions
{
    public abstract class Action
    {
        public abstract bool Invoke(Adventure currentGame, Command caller);
    }
}