using System.Collections.Generic;
using ConsoleApplication1.GameModel.Actions;

namespace ConsoleApplication1.GameModel
{
    public class Command
    {
        public int TargetId { get; set; }
        public List<string> Triggers { get; set; } = new List<string>();

        public Action Action { get; set; }
    }
}