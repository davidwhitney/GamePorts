using System.Collections.Generic;
using System.Linq;
using ConsoleApplication1.GameModel;

namespace ConsoleApplication1.Parsing
{
    public static class ChunkerExtensions
    {
        public static List<List<string>> SplitDataIntoChunks(this IEnumerable<string> lines)
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

        public static Dictionary<int, List<string>> ToDictionary(this List<string> chunk)
        {
            var targetDictionary = new Dictionary<int, List<string>>();
            foreach (var description in chunk)
            {
                var pair = Unpack(description);
                if (!pair.HasValue) continue;

                if (!targetDictionary.ContainsKey(pair.Value.Key))
                {
                    targetDictionary.Add(pair.Value.Key, new Location(pair.Value.Key));
                }

                targetDictionary[pair.Value.Key].Add(pair.Value.Value);
            }
            return targetDictionary;
        }

        public static KeyValuePair<int, string>? Unpack(string line)
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
}