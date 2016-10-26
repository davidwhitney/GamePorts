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
                var keyValue = description.Split('\t');

                int locationId;
                var locationIdAsString = keyValue[0];
                var foundInteger = int.TryParse(locationIdAsString, out locationId);

                if (!foundInteger) continue;

                if (keyValue.Length == 1)
                {
                    adventure.LongFormDescriptions.Add(locationId, new LocationDescription {""});
                }

                var textString = keyValue[1];
                adventure.LongFormDescriptions.Add(locationId, new LocationDescription {textString});
            }

            return adventure;
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
                currentBlock.Add(line);
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
        public Dictionary<int, LocationDescription> LongFormDescriptions { get; set; } = new Dictionary<int, LocationDescription>();
        
    }

    public class LocationDescription : List<string>
    {
        public override string ToString()
        {
            return string.Join(Environment.NewLine, this);
        }
    }
}