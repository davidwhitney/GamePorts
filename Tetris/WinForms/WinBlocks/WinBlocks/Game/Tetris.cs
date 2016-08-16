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
        public List<IPostProcessContent> PostProcessors => _renderer.PostProcessors;

        public int Height => _board.Height;
        public int Width => _board.Width;

        private readonly ISelectBlocks _selector;
        private readonly TetrisTextRenderer _renderer;
        private readonly TetrisGrid _board;
        

        public Tetris(ISelectBlocks selector)
        {
            _selector = selector;
            _renderer = new TetrisTextRenderer();
            _board = new TetrisGrid(10, 22);
        }

        public override string ToString()
        {
            var toDraw = new List<Tetrimino>();
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

            var downwardsMovement = new Delta { Y = +1 };
            if (!AllBlocksInTetriminoCanMove(Current, downwardsMovement))
            {
                Lock();
                ClearAnyCompleteLines();
                return;
            }

            Move(Direction.Down);
        }

        private void Lock()
        {
            foreach (var location in Current.BlockLocations)
            {
                _board.SetValue(location.X, location.Y, location);
            }
            Current = null;
        }

        private void ClearAnyCompleteLines()
        {
            foreach (var row in _board.RawRows)
            {
                if (row.Any(x => x.Content == "."))
                {
                    continue;
                }

                foreach (var cell in row)
                {
                    cell.Content = ".";
                }
            }
        }

        public void Move(Direction direction)
        {
            if (Current == null)
            {
                return;
            }

            if (!AllBlocksInTetriminoCanMove(Current, direction.ToDelta()))
            {
                return;
            }

            var target = Current.Location.From(direction.ToDelta());
            Current.ShiftTo(target.X, target.Y);
        }

        public void Rotate(Direction direction)
        {
            Current?.Rotate(direction);
        }

        private bool AllBlocksInTetriminoCanMove(Tetrimino t, Delta delta)
        {
            return t.BlockLocations.All(loc => CanMoveInto(loc.From(delta)));
        }

        // BUG: This needs to check each individual piece.
        private bool CanMoveInto(Location loc)
        {
            return CanMoveInto(loc.X, loc.Y);
        }

        private bool CanMoveInto(int x, int y)
        {
            if (x < 0 || y < 0)
            {
                return false;
            }

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
    }

    public class Delta : Location { }

    public static class MovementExtensions
    {
        public static Delta ToDelta(this Direction direction)
        {
            var delta = new Delta();

            if (direction == Direction.Right)
            {
                delta.X = 1;
            }

            if (direction == Direction.Left)
            {
                delta.X = -1;
            }

            if (direction == Direction.Down)
            {
                delta.Y = 1;
            }

            return delta;
        }
    }
}