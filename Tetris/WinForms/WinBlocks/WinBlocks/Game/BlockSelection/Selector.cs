using System;
using System.Collections.Generic;
using WinBlocks.Game.Model;

namespace WinBlocks.Game.BlockSelection
{
    public class Selector : ISelectBlocks
    {
        private readonly Random _rng;
        private readonly List<Tetrimino> _options;

        public Selector(List<Tetrimino> options)
        {
            _rng = new Random();
            _options = options;
        }

        public Tetrimino Random(int x = 0, int y = 0)
        {
            var index = _rng.Next(0, _options.Count - 1);
            return ((Tetrimino) _options[index].Clone()).ShiftTo(x, y);
        }
    }
}