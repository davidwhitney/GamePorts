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
                    adventure.Locations[key].ShortForm = shortForms[key].ToString();
                }
            }

            var locationMap = chunks.Count > 2 ? chunks.Skip(2).First() : new List<string>();
            foreach (var line in locationMap)
            {
                var pathData = new Queue<int>(line.Split('\t').Select(int.Parse));
                var thisLocation = adventure.Locations[pathData.Dequeue()];
                var pathRecord = new LocationRefs {LocationId = pathData.Dequeue()};

                while (pathData.Any())
                {
                    pathRecord.Action.Add(pathData.Dequeue().ToString());
                }
                
                thisLocation.Paths.Add(pathRecord);
            }

            return adventure;
        }
    }
}