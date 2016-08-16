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
        public List<IPostProcessContent> PostProcessors => _renderer.PostProcessors;

        private Tetrimino _current;
        private readonly ISelectBlocks _selector;
        private readonly TetrisTextRenderer _renderer;
        private readonly TetrisGrid _board;

        public Tetris(ISelectBlocks selector, List<string> gridRows = null, Tetrimino active = null)
        {
            _selector = selector;
            _renderer = new TetrisTextRenderer();
            _board = gridRows == null ? new TetrisGrid(10, 22) : new TetrisGrid(gridRows);
            _current = active;
        }

        public override string ToString()
        {
            return _renderer.Render(_board, _current).Trim();
        }

        public void Step()
        {
            if (_current == null)
            {
                _current = _selector.Random();
                return;
            }

            if (!CanMoveInto(_current, new Delta { Y = +1 }))
            {
                Lock();
                ClearAnyCompleteLines();
                return;
            }

            Move(Direction.Down);
        }

        private void Lock()
        {
            _current.BlockLocations.ForEach(loc => _board.SetValue(loc.X, loc.Y, loc));
            _current = null;
        }

        private void ClearAnyCompleteLines()
        {
            var rowsToClear = new List<int>();

            for (var y = _board.RawRows.Count - 1; y >= 0; y--)
            {
                var row = _board.RawRows[y];
                if (row.Any(x => x.Content == "."))
                {
                    continue;
                }

                rowsToClear.Add(y);
            }

            rowsToClear.ForEach(c => _board.RawRows.RemoveAt(c));
            rowsToClear.ForEach(c => _board.RawRows.Insert(0, _board.CreateRow()));
        }

        public void Move(Direction direction)
        {
            if (_current == null)
            {
                return;
            }

            if (!CanMoveInto(_current, direction.ToDelta()))
            {
                return;
            }

            var target = _current.BoundingBoxLocation.From(direction.ToDelta());
            _current.ShiftTo(target.X, target.Y);
        }

        public void Rotate(Direction direction)
        {
            _current?.Rotate(direction);
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