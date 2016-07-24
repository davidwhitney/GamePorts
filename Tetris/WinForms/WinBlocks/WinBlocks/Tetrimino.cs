using System;
using System.Collections.Generic;
using System.Linq;

namespace WinBlocks
{
    public class Tetrimino
    {
        public string Id => Pattern.First(c => c != '.').ToString();
        public string Pattern { get; set; }
        public int Y { get; set; }
        public int X { get; set; }

        public Tetrimino(string pattern)
        {
            Pattern = pattern;
        }

        public List<string> PatternParts => Pattern.Split(new[] {Environment.NewLine}, StringSplitOptions.None).ToList();

        public IEnumerable<RenderLocation> BlockLocations()
        {
            for (var xx = 0; xx < PatternParts.Count; xx++)
            {
                var row = PatternParts[xx];

                for (var yy = 0; yy < row.Length; yy++)
                {
                    var c = row[yy];

                    if (c == '.')
                    {
                        continue;
                    }

                    yield return new RenderLocation
                    {
                        Content = c.ToString(),
                        X = X + xx,
                        Y = Y + yy
                    };
                }
            }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Tetrimino);
        }

        protected bool Equals(Tetrimino other)
        {
            return string.Equals(Pattern, other.Pattern) && Y == other.Y && X == other.X;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Pattern?.GetHashCode() ?? 0;
                hashCode = (hashCode*397) ^ Y;
                hashCode = (hashCode*397) ^ X;
                return hashCode;
            }
        }
    }
}