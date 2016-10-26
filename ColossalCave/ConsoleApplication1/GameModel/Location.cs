using System;
using System.Collections.Generic;

namespace ConsoleApplication1.GameModel
{
    public class Location : List<string>
    {
        public int Id { get; set; }
        public string ShortForm { get; set; }
        public List<Command> Paths { get; set; } = new List<Command>();

        public Location(int id)
        {
            Id = id;
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, this);
        }
    }
}