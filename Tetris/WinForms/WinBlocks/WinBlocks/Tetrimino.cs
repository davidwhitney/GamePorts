using System;
using System.Collections.Generic;
using System.Linq;

namespace WinBlocks
{
    public class Tetrimino
    {
        public string Id => Pattern.First(c => c != '.').ToString();
        public string Pattern { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }

        public Tetrimino(string pattern)
        {
            Pattern = pattern;
        }

        public List<string> PatternParts => Pattern.Split(new[] {Environment.NewLine}, StringSplitOptions.None).ToList();
        public List<Cell> LocationMap { get; set; }
    }

    public class Cell
    {
        public int? X { get; set; }
        public int? Y { get; set; }
    }
}