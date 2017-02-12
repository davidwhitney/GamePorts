using System.Collections.Generic;

namespace ConsoleApplication1.GameModel.Actions
{
    public class NavigateAction : Action
    {
        public List<IActionConstraint> Constraints = new List<IActionConstraint>();

        public override bool Invoke(Adventure currentGame, Command caller)
        {
            currentGame.CurrentLocation = caller.TargetId;
            return true;
        }
    }
}