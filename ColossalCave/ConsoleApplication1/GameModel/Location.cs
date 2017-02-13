using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApplication1.GameModel
{
    public class Location : List<string>
    {
        public int Id { get; set; }
        public string ShortForm { get; set; }
        public List<Command> Actions { get; set; } = new List<Command>();

        public Location(int id)
        {
            Id = id;
        }

        public override string ToString()
        {
#if DEBUG
            var sb = new StringBuilder("Triggers\r\n");
            foreach (var action in Actions)
            {
                foreach (var trigger in action.Triggers)
                {
                    sb.AppendLine($"\t{trigger.Id}: {trigger.Word} leads to {action?.TargetId}:{action?.TargetRef?.ShortForm}");
                }
            }

            return $"{Description}\r\n" +
                   $"Location: {Id}\r\n" +
                   $"ShortForm: {ShortForm}\r\n" +
                   $"{sb}";
#endif

            return Description;
        }

        public string Description => string.Join(Environment.NewLine, this);

        public Location WithActions(Action<List<Command>> actions)
        {
            actions(Actions);
            return this;
        }
        
        public TActionType GetAction<TActionType>(int i) where TActionType : Actions.Action
        {
            return Actions[i].Action as TActionType;
        }
    }
}