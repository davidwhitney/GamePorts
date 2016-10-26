namespace ConsoleApplication1.GameModel.Actions
{
    public abstract class Action
    {
        public abstract void Invoke(Adventure currentGame, Command caller);
    }
}