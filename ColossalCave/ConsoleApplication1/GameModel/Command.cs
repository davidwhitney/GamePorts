using System.Collections.Generic;

namespace ConsoleApplication1.GameModel
{
    public class Command
    {
        public int TargetId { get; set; }
        public List<string> Action { get; set; } = new List<string>();
    }
}