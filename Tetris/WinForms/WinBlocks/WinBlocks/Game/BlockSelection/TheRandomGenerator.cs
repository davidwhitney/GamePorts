using System;
using System.Collections.Generic;
using System.Linq;
using WinBlocks.Game.Model;

namespace WinBlocks.Game.BlockSelection
{
    public class TheRandomGenerator : ISelectBlocks
    {
        private readonly List<Tetrimino> _options;
        private Queue<Tetrimino> _bag = new Queue<Tetrimino>();
        
        public TheRandomGenerator(List<Tetrimino> options)
        {
            _options = options;
        }

        public virtual Tetrimino Random(int x = 0, int y = 0)
        {
            if (_bag.Count == 0)
            {
                var shuffle = Shuffle();
                _bag = new Queue<Tetrimino>(shuffle);
            }

            var template = _bag.Dequeue();
            return ((Tetrimino)template.Clone()).ShiftTo(x, y);
        }

        protected virtual IEnumerable<Tetrimino> Shuffle()
        {
            return _options.OrderBy(_ => Guid.NewGuid());
        }
    }
}