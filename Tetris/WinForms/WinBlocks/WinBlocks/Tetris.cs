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
        public List<IPostProcessContent> PostProcessors = new List<IPostProcessContent>();

        public int Height => Rows.Count;
        public int Width => Rows.First().Length;

        public static string EmptyBoard => string.Join(Environment.NewLine, BoardRows);
        private static List<string> BoardRows => new List<string>(Enumerable.Repeat("..........", 22));

        public Tetris(ISelectBlocks selector)
        {
            _selector = selector;
            Rows = BoardRows;
        }

        public override string ToString()
        {
            var snapshot = new List<string>();

            var toDraw = new List<Tetrimino>(BoardContents);
            if (Current != null)
            {
                toDraw.Add(Current);
            }

            var blocks = toDraw.SelectMany(x => x.BlockLocations()).ToList();

            for (var y = 0; y < Height; y++)
            {
                var sbLine = new StringBuilder();

                for (var x = 0; x < Width; x++)
                {
                    var inLoc = blocks.SingleOrDefault(l => l.X == x && l.Y == y);
                    var token = inLoc != null ? inLoc.Content : ".";
                    token = PostProcessors.Aggregate(token, (current, processors) => processors.Process(current));

                    sbLine.Append(token);
                }

                snapshot.Add(sbLine.ToString());
            }

            return string.Join(Environment.NewLine, snapshot);
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

            return locs.All(loc => CanMoveInto(loc.X, loc.Y + 1));
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

            if (CollidesWithExistingPiece(x, y))
            {
                return false;
            }

            return true;
        }

        private bool CollidesWithExistingPiece(int x, int y)
        {
            foreach (var piece in BoardContents)
            {
                var elements = piece.BlockLocations().ToList();
                if (elements.Any(el => el.X == x && el.Y == y))
                {
                    return true;
                }
            }
            return false;
        }

        public void Move(Direction direction)
        {
            if (Current == null)
            {
                return;
            }

            if (direction == Direction.Right)
            {
                Current.X++;
            }

            if (direction == Direction.Left)
            {
                Current.X--;
            }
        }
    }

    public enum Direction
    {
        Left,
        Right,
        Down
    }
}