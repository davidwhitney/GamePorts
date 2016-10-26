using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;

namespace ConsoleApplication1
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

            var chunks = SplitDataIntoChunks(lines);
            var longForms = chunks.First();
            foreach (var description in longForms)
            {
                var pair = Unpack(description);
                if (!pair.HasValue) continue;

                if (!adventure.Locations.ContainsKey(pair.Value.Key))
                {
                    adventure.Locations.Add(pair.Value.Key, new LocationDescription());
                }
                
                adventure.Locations[pair.Value.Key].Add(pair.Value.Value);
            }

            if (chunks.Count < 2)
            {
                return adventure;
            }

            var shortForms = chunks.Skip(1).First();
            foreach (var description in shortForms)
            {
                var pair = Unpack(description);
                if (!pair.HasValue) continue;

                adventure.Locations[pair.Value.Key].ShortForm = pair.Value.Value;
            }

            return adventure;
        }

        public KeyValuePair<int, string>? Unpack(string line)
        {
            var keyValue = line.Split('\t');

            int locationId;
            var locationIdAsString = keyValue[0];
            var foundInteger = int.TryParse(locationIdAsString, out locationId);
            if (!foundInteger)
            {
                return null;
            }
            var textString = keyValue.Length == 1 ? "" : keyValue[1];
            return new KeyValuePair<int, string>(locationId, textString);
        }

        private static List<List<string>> SplitDataIntoChunks(IEnumerable<string> lines)
        {
            var chunks = new List<List<string>>();
            var currentBlock = new List<string>();
            foreach (var line in lines)
            {
                if (line == "-1")
                {
                    chunks.Add(currentBlock);
                    currentBlock = new List<string>();
                }
                else
                {
                    currentBlock.Add(line);
                }
            }

            if (currentBlock.Any() 
                && !chunks.Contains(currentBlock))
            {
                chunks.Add(currentBlock);
            }

            return chunks;
        }
    }

    public class Adventure
    {
        public int RawLength { get; set; }
        public Dictionary<int, LocationDescription> Locations { get; set; } = new Dictionary<int, LocationDescription>();
        
    }

    public class LocationDescription : List<string>
    {
        public string ShortForm { get; set; }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, this);
        }
    }
}