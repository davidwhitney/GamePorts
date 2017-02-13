using System.Collections.Generic;
using System.Linq;

namespace ConsoleApplication1.GameModel
{
    public class Adventure
    {
        public int RawLength { get; set; }
        public Dictionary<int, Location> Locations { get; set; } = new Dictionary<int, Location>();

        public int CurrentLocation { get; set; }
        public string CurrentLocationText => Locations[CurrentLocation].ToString();

        public List<int> InventoryItems { get; set; }
        public List<VocabularyItem> Vocabulary { get; set; } = new List<VocabularyItem>();

        public Adventure StartGame()
        {
            CurrentLocation = Locations.First().Key;
            return this;
        }

        public List<string> ProcessInput(string input)
        {
            var current = Locations[CurrentLocation];
            var validMoves = current.Actions;
            var matchingActions = validMoves.Where(x => x.Triggers.Any(t=>t.Word == input)).ToList();

            if (!matchingActions.Any())
            {
                return new List<string> {"I don't understand that."};
            }

            var successfulAction = matchingActions.FirstOrDefault(x => x.Action.Invoke(this, x));

            return new List<string> { CurrentLocationText };
        }
    }
}