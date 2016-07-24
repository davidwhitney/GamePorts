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
                foreach (var renderLocation in item.BlockLocations())
                {
                    snapshot[renderLocation.Y] = snapshot[renderLocation.Y].Replace(renderLocation.X, renderLocation.Content);
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
                Current.Y = 0;
                Current.X = 1;
                return;
            }

            if (!CurrentPieceCanMove())
            {
                BoardContents.Push(Current);
                Current = null;
                return;
            }

            Current.Y++;
        }

        private bool CurrentPieceCanMove()
        {
            var locs = Current.BlockLocations().ToList();

            var lowestElement = locs.Max(l => l.Y);
            if (lowestElement + 1 >= Rows.Count)
            {
                return false;
            }

            var allBlocksCanMoveDown = locs.All(loc => CanMoveInto(loc.X, loc.Y + 1));
            return allBlocksCanMoveDown;
        }

        private bool CanMoveInto(int x, int y)
        {
            if (y >= Rows.Count)
            {
                return false;
            }

            if (x >= Rows[y].Length)
            {
                return false;
            }

            return Rows[y][x].ToString() == ".";
        }
    }
}
