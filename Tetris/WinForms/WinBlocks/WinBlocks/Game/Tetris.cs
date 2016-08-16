using System;
using System.Collections.Generic;
using System.Linq;
using WinBlocks.Game.BlockSelection;
using WinBlocks.Game.Input;
using WinBlocks.Game.Model;
using WinBlocks.Game.Rendering;

namespace WinBlocks.Game
{
    public class Tetris
    {
        public List<string> Rows
        {
            get { return _board.Rows; }
            set { _board.Rows = value; }
        }

        public Tetrimino Current { get; set; }
        public Stack<Tetrimino> BoardContents { get; } = new Stack<Tetrimino>();
        public IEnumerable<RenderLocation> OccupiedLocations => BoardContents.SelectMany(x => x.BlockLocations).ToList();
        public List<IPostProcessContent> PostProcessors => _renderer.PostProcessors;

        public int Height => _board.Height;
        public int Width => _board.Width;

        private readonly ISelectBlocks _selector;
        private readonly TetrisTextRenderer _renderer;
        private readonly TetrisGrid _board;

        public static string EmptyBoard => string.Join(Environment.NewLine, BoardRows);
        private static List<string> BoardRows => new List<string>(Enumerable.Repeat("..........", 22));

        public Tetris(ISelectBlocks selector)
        {
            _selector = selector;
            _renderer = new TetrisTextRenderer();
            _board = new TetrisGrid(10, 22);
        }

        public override string ToString()
        {
            var toDraw = new List<Tetrimino>(BoardContents);
            if (Current != null)
            {
                toDraw.Add(Current);
            }

            return _renderer.Render(Width, Height, toDraw, _board, Current);
        }

        public void Step()
        {
            if (Current == null)
            {
                Current = _selector.Random();
                return;
            }

            if (!Current.BlockLocations.All(loc => CanMoveInto(loc.X, loc.Y + 1)))
            {
                Lock();
                ClearAnyCompleteLines();
                return;
            }

            Move(Direction.Down);
        }

        private void Lock()
        {
            BoardContents.Push(Current);

            foreach (var location in Current.BlockLocations)
            {
                _board.SetValue(location.X, location.Y, location);
            }

            Current = null;
        }

        private void ClearAnyCompleteLines()
        {
            var allBlockLocs = OccupiedLocations.ToList();
            for (var y = 0; y < Height; y++)
            {
                var blocksOnThisY = allBlockLocs.Where(b => b.Y == y).ToList();
                if (blocksOnThisY.Count != Width)
                {
                    continue;
                }

                foreach (var block in blocksOnThisY)
                {
                    foreach (var tet in BoardContents)
                    {
                        tet.BlockLocations.RemoveAll(l => tet.BlockLocations.Where(location => location.Equals(block)).ToList().Contains(l));
                    }
                }
            }
        }

        public void Move(Direction direction)
        {
            if (Current == null)
            {
                return;
            }
            
            var target = EstablishTarget(direction);
            if (!CanMoveInto(target.X, target.Y))
            {
                return;
            }

            Current.ShiftTo(target.X, target.Y);
        }

        public void Rotate(Direction direction)
        {
            Current?.Rotate(direction);
        }

        private bool CanMoveInto(int x, int y)
        {
            if (y >= Height)
            {
                return false;
            }

            if (x >= Width)
            {
                return false;
            }

            var targetLocation = _board.ValueAt(x, y);
            return targetLocation.Content == ".";
        }

        private Location EstablishTarget(Direction direction)
        {
            var targetLocation = new Location {X = Current.X, Y = Current.Y};

            if (direction == Direction.Right)
            {
                targetLocation.X++;
            }

            if (direction == Direction.Left)
            {
                targetLocation.X--;
            }

            if (direction == Direction.Down)
            {
                targetLocation.Y++;
            }

            return targetLocation;
        }
    }
}