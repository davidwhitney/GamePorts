using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using ConsoleApplication1.GameModel;
using ConsoleApplication1.GameModel.Actions;
using Action = ConsoleApplication1.GameModel.Actions.Action;

namespace ConsoleApplication1.Parsing
{
    public class Parser
    {
        private readonly IFileSystem _fs;

        public Parser(IFileSystem fs)
        {
            _fs = fs;
        }

        public Adventure Parse(string dataPath)
        {
            if (string.IsNullOrWhiteSpace(dataPath))
            {
                throw new ArgumentNullException(nameof(dataPath));
            }

            var lines = _fs.File.ReadAllLines(dataPath);
            var adventure = new Adventure
            {
                RawLength = lines.Length
            };

            var chunks = lines.SplitDataIntoChunks();
            var longForms = chunks.First().ToDictionary();
            var shortForms = (chunks.Count > 1 ? chunks.Skip(1).First() : new List<string>()).ToDictionary();

            var allKeys = longForms.Keys;
            foreach (var key in allKeys)
            {
                var value = longForms[key];
                adventure.Locations.Add(key, new Location(key));
                adventure.Locations[key].AddRange(value);

                if (shortForms.ContainsKey(key))
                {
                    var shortForm = string.Join(Environment.NewLine, shortForms[key]);
                    adventure.Locations[key].ShortForm = shortForm;
                }
            }

            BuildLocationMap(chunks, adventure);

            return adventure;
        }

        private static void BuildLocationMap(ICollection<List<string>> chunks, Adventure adventure)
        {
            var vocabulary = chunks.Count > 3 ? chunks.Skip(3).First() : new List<string>();
            foreach (var line in vocabulary)
            {
                var parts = line.Split('\t');
                var n = int.Parse(parts[0]);
                var m = n / 1000;

                var vocab = new VocabularyItem
                {
                    Word = parts[1],
                    IsMotion = m == 0,
                    IsObject = m == 1,
                    IsAction = m == 2,
                    IsSpecial = m == 3,
                    SpecialIndex = n % 1000,
                    IsTreasure = m == 1 && n % 1000 >= 50 && n % 1000 <= 79
                };

                adventure.Vocabulary.Add(vocab);
            }

            var locationMap = chunks.Count > 2 ? chunks.Skip(2).First() : new List<string>();
            foreach (var line in locationMap)
            {
                var rawData = line.Split('\t').Select(int.Parse).ToList();
                var pathData = new Queue<int>(rawData);

                var thisLocation = adventure.Locations[pathData.Dequeue()];

                var secondValue = pathData.Count > 0 ? pathData.Dequeue() : thisLocation.Id;
                var targetId = secondValue%1000;
                var motionConditions = secondValue / 1000;

                var command = CreateCommand(targetId, secondValue);
                command.Action = ConstrainMovementOptions(motionConditions);

                while (pathData.Any())
                {
                    var item = pathData.Dequeue();
                    var word = adventure.Vocabulary.Count > item ? adventure.Vocabulary[item].Word : null;
                    var vocab = adventure.Vocabulary.Count > item ? adventure.Vocabulary[item] : null;
                    var trigger = new Trigger {Id = item, Word = word, VocabularyRef = vocab };
                    command.Triggers.Add(trigger);
                }

                thisLocation.Actions.Add(command);
            }

            foreach (var location in adventure.Locations)
            {
                foreach (var action in location.Value.Actions)
                {
                    action.TargetRef = adventure.Locations.Keys.Contains(action.TargetId) ? adventure.Locations[action.TargetId] : null;
                }
            }
        }
        
        private static Action ConstrainMovementOptions(int motionConditions)
        {
            if (motionConditions == 0)
            {
                return new NavigateAction();
            }

            var action = new NavigateAction();
            if (motionConditions > 0
                && motionConditions < 100)
            {
                action.Constraints.Add(new PercentageConstraint {Percentage = motionConditions});
            }

            if (motionConditions == 100)
            {
                action.Constraints.Add(new ForbiddenToDwarfsConstraint());
            }

            if (motionConditions > 100
                && motionConditions < 200)
            {
                action.Constraints.Add(new InventoryConstraint {ItemId = motionConditions - 100});
            }

            if (motionConditions > 200
                && motionConditions < 300)
            {
                action.Constraints.Add(new ItemOrRoomPresentConstraint {ItemId = motionConditions - 200});
            }

            if (motionConditions > 300)
            {
                action.Constraints.Add(new ModuloConstraint {ModuloResultMustNotBeEqualTo = motionConditions/100 - 3});
            }
            return action;
        }

        private static Command CreateCommand(int targetId, int secondValue)
        {
            Command pathRecord;
            if (targetId > 300 && targetId <= 500)
            {
                pathRecord = new ComputedGoTo {TargetId = secondValue - 300};
            }
            else if (targetId > 500)
            {
                pathRecord = new Message {TargetId = secondValue - 500};
            }
            else
            {
                pathRecord = new Navigate {TargetId = targetId};
            }
            return pathRecord;
        }
    }
}
 