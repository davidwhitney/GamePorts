using System;
using System.Collections.Generic;
using System.Linq;
using WinBlocks.Game.Model;

namespace WinBlocks.Game.BlockSelection
{
    public class GrandmasterAceGenerator : TheRandomGenerator
    {
        private bool _drawnAny;

        public GrandmasterAceGenerator(List<Tetrimino> options) : base(options)
        {
        }

        protected override IEnumerable<Tetrimino> Shuffle()
        {
            if (!_drawnAny)
            {
                var shuffled = base.Shuffle().ToList();
                var firstCandidates = new List<string> {"I", "J", "L", "T"};
                var firstOne = firstCandidates.OrderBy(_ => Guid.NewGuid()).First();

                var firstTet = shuffled.Single(t => t.Id == firstOne);
                shuffled.Remove(firstTet);
                shuffled.Insert(0, firstTet);

                _drawnAny = true;
                return shuffled;
            }

            return base.Shuffle();
        }
    }
}