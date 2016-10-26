using System.Collections.Generic;

namespace ConsoleApplication1
{
    public class Adventure
    {
        public int RawLength { get; set; }
        public Dictionary<int, Location> Locations { get; set; } = new Dictionary<int, Location>();
    }
}