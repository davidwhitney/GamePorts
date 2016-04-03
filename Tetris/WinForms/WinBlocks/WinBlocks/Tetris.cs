using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinBlocks
{
    public class Tetris
    {
        private readonly ISelectBlocks _selector;
        private List<string> _rows;
        public Tetrimino Current { get; set; }
        public Stack<Tetrimino> BoardContents { get; } = new Stack<Tetrimino>();

        public int Height => _rows.Count - 2;
        public int Width => _rows.First().Length;

        public Tetris(ISelectBlocks selector, string pattern = "")
        {
            _selector = selector;
            _rows = pattern == "" ? new List<string>(Enumerable.Repeat("..........", 22)) : RowsFromPattern(pattern);
        }

        public override string ToString()
        {
            var snapshot = new List<string>(_rows);

            var toDraw = new List<Tetrimino>(BoardContents);
            if (Current != null)
            {
                toDraw.Add(Current);
            }

            foreach (var item in toDraw)
            {
                var targetRowOffset = item.Row;
                foreach (var patternLine in item.PatternParts)
                {
                    snapshot[targetRowOffset] = snapshot[targetRowOffset].Insert(item.Column, patternLine);
                    snapshot[targetRowOffset] = snapshot[targetRowOffset].Remove(item.Column + patternLine.Length, patternLine.Length);

                    targetRowOffset++;
                }
            }

            var buffer = new StringBuilder();
            foreach (var row in snapshot.Skip(2))
            {
                buffer.AppendLine(row);
            }

            return buffer.ToString();
        }

        public void Step()
        {
            if (Current == null)
            {
                Current = _selector.Random();
                Current.Row = 0;
                Current.Column = 1;
            }

            if (Current.Row < _rows.Count - 1)
            {
                Current.Row++;
            }
            else
            {
                BoardContents.Push(Current);
                Current = null;
            }
        }

        private List<string> RowsFromPattern(string pattern)
        {
            var lines = pattern.Trim().Split(new[] {Environment.NewLine}, StringSplitOptions.None);
            var ourRows =  new List<string>(lines);
            var topPad = new string('.', lines.First().Length);
            ourRows.Insert(0, topPad);
            ourRows.Insert(0, topPad);

            var leftMost = int.MaxValue;
            int topMost = 0;
            var shapeLines = new List<string>();
            for (int rowIndex = 0; rowIndex < ourRows.Count; rowIndex++)
            {
                var row = ourRows[rowIndex];
                var shapeRow = "";
                for (int index = 0; index < row.Length; index++)
                {
                    var letter = row[index];

                    if (letter != '.')
                    {
                        leftMost = index < leftMost ? index : leftMost;
                        topMost = rowIndex > topMost ? rowIndex : topMost;
                        shapeRow += letter;

                    }
                }
                if (!string.IsNullOrWhiteSpace(shapeRow))
                {
                    shapeLines.Add(shapeRow);
                }
            }
            if (shapeLines.Any())
            {
                Current = new Tetrimino(string.Join(Environment.NewLine, shapeLines))
                {
                    Column = leftMost,
                    Row = topMost
                };
            }

            return ourRows;
        }
    }

    public class Tetrominoes : List<Tetrimino>
    {

        public Tetrominoes()
        {

            Add(new Tetrimino("IIII"));

            Add(new Tetrimino("OO" + Environment.NewLine +
                              "OO"));

            Add(new Tetrimino(".T." + Environment.NewLine +
                              "TTT"));

            Add(new Tetrimino(".SS" + Environment.NewLine +
                              "SS."));

            Add(new Tetrimino("ZZ." + Environment.NewLine +
                              ".ZZ"));

            Add(new Tetrimino("J.." + Environment.NewLine +
                              "JJJ"));

            Add(new Tetrimino("..L" + Environment.NewLine +
                              "LLL"));
        }
    }

    public interface ISelectBlocks
    {
        Tetrimino Random();
    }

    public class Selector : ISelectBlocks
    {
        private readonly Random _rng;
        private readonly List<Tetrimino> _options;

        public Selector(List<Tetrimino> options)
        {
            _rng = new Random();
            _options = options;
        }

        public Tetrimino Random()
        {
            var index = _rng.Next(0, _options.Count - 1);
            return _options[index];
        }
    }

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
    }
}
