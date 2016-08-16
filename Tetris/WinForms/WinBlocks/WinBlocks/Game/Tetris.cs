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
            return _renderer.Render(_board, Current);
        }

        public void Step()
        {
            if (Current == null)
            {
                Current = _selector.Random();
                return;
            }

            if (!CanMoveInto(Current, new Delta { Y = +1 }))
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

            if (!CanMoveInto(Current, direction.ToDelta()))
            {
                return;
            }

            var target = Current.BoundingBoxLocation.From(direction.ToDelta());
            Current.ShiftTo(target.X, target.Y);
        }

        public void Rotate(Direction direction)
        {
            Current?.Rotate(direction);
        }

        private bool CanMoveInto(Tetrimino t, Delta delta)
        {
            return t.BlockLocations.All(loc =>
            {
                var location = loc.From(delta);
                return _board.IsOccupied(location.X, location.Y);
            });
        }
    }
}