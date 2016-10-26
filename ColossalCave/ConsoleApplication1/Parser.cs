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
                adventure.Locations.Add(key, new LocationDescription());
                adventure.Locations[key].AddRange(value);

                if (shortForms.ContainsKey(key))
                {
                    adventure.Locations[key].ShortForm = shortForms[key].ToString();
                }
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