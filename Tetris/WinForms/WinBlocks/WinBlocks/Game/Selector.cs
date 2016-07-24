using System;
using System.Collections.Generic;

namespace WinBlocks.Game
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

        public Tetrimino Random()
        {
            var index = _rng.Next(0, _options.Count - 1);
            return new Tetrimino(_options[index].Pattern);
        }
    }
}