using System.Collections.Generic;
using System.Linq;

namespace ConsoleApplication1.GameModel
{
    public class Adventure
    {
        public int RawLength { get; set; }
        public Dictionary<int, Location> Locations { get; set; } = new Dictionary<int, Location>();

        public int CurrentLocation { get; set; }
        public List<int> InventoryItems { get; set; }

        public Adventure StartGame()
        {
            CurrentLocation = Locations.First().Key;
            return this;
        }

        public List<string> ProcessInput(string input)
        {
            var current = Locations[CurrentLocation];
            var validMoves = current.Actions;
            var validAction = validMoves.Where(x => x.Triggers.Contains(input)).ToList();

            if (!validAction.Any())
            {
                return new List<string> {"I don't understand that."};
            }

            foreach (var action in validAction)
            {
                var result = action.Action.Invoke(this, action);
                if (result)
                {
                    break;
                }
            }


            return new List<string>();
        }
    }
}