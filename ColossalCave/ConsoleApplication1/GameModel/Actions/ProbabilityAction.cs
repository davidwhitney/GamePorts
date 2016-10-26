namespace ConsoleApplication1.GameModel.Actions
{
    public class ProbabilityAction : NavigateAction
    {
        public int Percentage { get; set; }

        public override void Invoke(Adventure currentGame, Command caller)
        {
            base.Invoke(currentGame, caller);
        }
    }
}