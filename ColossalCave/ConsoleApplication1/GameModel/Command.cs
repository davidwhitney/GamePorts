using System;
using System.Collections.Generic;
using Action = ConsoleApplication1.GameModel.Actions.Action;

namespace ConsoleApplication1.GameModel
{
    public class Command
    {
        public int TargetId { get; set; }
        public List<Trigger> Triggers { get; set; } = new List<Trigger>();

        public Action Action { get; set; }
        public Location TargetRef { get; set; }
    }

    public class Trigger
    {
        public int Id { get; set; }
        public string Word { get; set; }
        public VocabularyItem VocabularyRef { get; set; }
    }
}