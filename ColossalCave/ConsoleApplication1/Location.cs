using System;
using System.Collections.Generic;

namespace ConsoleApplication1
{
    public class Location : List<string>
    {
        public int Id { get; set; }
        public string ShortForm { get; set; }
        public List<LocationRefs> Paths { get; set; } = new List<LocationRefs>();

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