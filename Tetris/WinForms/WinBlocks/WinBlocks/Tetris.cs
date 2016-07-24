using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinBlocks
{
    public class Tetris
    {
        private readonly ISelectBlocks _selector;
        public List<string> Rows { get; set; }
        public Tetrimino Current { get; set; }
        public Stack<Tetrimino> BoardContents { get; } = new Stack<Tetrimino>();

        public int Height => Rows.Count;
        public int Width => Rows.First().Length;

        public static string EmptyBoard => string.Join(Environment.NewLine, BoardRows);
        private static List<string> BoardRows => new List<string>(Enumerable.Repeat("..........", 22));

        public Tetris(ISelectBlocks selector, string pattern = "")
        {
            _selector = selector;

            if (pattern == "")
            {
                Rows = BoardRows;
            }
            else
            {
                var state = new BoardBuilder().Populate(pattern);
                Rows = state.Item1;
                Current = state.Item2;
            }
        }

        public override string ToString()
        {
            var snapshot = new List<string>(Rows);

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
            foreach (var row in snapshot)
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
                return;
            }

            if (Current.Row >= Rows.Count - 1)
            {
                BoardContents.Push(Current);
                Current = null;
                return;
            }

            Current.Row++;
        }

       
    }
}
