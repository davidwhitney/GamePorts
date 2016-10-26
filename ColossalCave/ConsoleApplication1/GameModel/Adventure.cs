using System.Collections.Generic;

namespace ConsoleApplication1.GameModel
{
    public class Adventure
    {
        public int RawLength { get; set; }
        public Dictionary<int, Location> Locations { get; set; } = new Dictionary<int, Location>();
    }
}