namespace ConsoleApplication1.GameModel.Actions
{
    public class NavigateAction : Action
    {
        public override void Invoke(Adventure currentGame, Command caller)
        {
            currentGame.CurrentLocation = caller.TargetId;
        }
    }
}