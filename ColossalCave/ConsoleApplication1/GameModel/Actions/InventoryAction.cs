namespace ConsoleApplication1.GameModel.Actions
{
    public class InventoryAction : NavigateAction
    {
        public int ItemId { get; set; }

        public override void Invoke(Adventure currentGame, Command caller)
        {
            base.Invoke(currentGame, caller);
        }
    }
}