using System;
using System.Collections.Generic;
using System.Linq;
using WinBlocks.Game.Model;

namespace WinBlocks.Game.BlockSelection
{
    public class AlexGenerator : ISelectBlocks
    {
        private readonly Random _rng;
        private readonly Dictionary<string, Tetrimino> _candidates = new Dictionary<string, Tetrimino>();
        private readonly List<Tetrimino> _lastTetsArrayUpToTwelve = new List<Tetrimino>();

        public AlexGenerator(List<Tetrimino> options)
        {
            _rng = new Random();

            foreach (var option in options)
            {
                _candidates.Add(option.Id, option);
            }
        }

        public Tetrimino Random(int x = 0, int y = 0)
        {
            if (_lastTetsArrayUpToTwelve.All(t => t.Id != "I") && _lastTetsArrayUpToTwelve.Count >= 12)
            {
                return NewTetrimino("I", x, y);
            }

            var lastFour = _lastTetsArrayUpToTwelve.Take(4).ToList();
            if (lastFour.All(t => t.Id == "S" || t.Id == "Z"))
            {
                var smallerDictionary = DictionaryWithoutSorZ();
                return NewTetrimino(KeyFrom(smallerDictionary), x, y);
            }
            
            return NewTetrimino(KeyFrom(_candidates), x, y);
        }

        private Dictionary<string, Tetrimino> DictionaryWithoutSorZ()
        {
            var smallerDictionary = new Dictionary<string, Tetrimino>(_candidates);
            smallerDictionary.Remove("S");
            smallerDictionary.Remove("Z");
            return smallerDictionary;
        }

        private string KeyFrom(Dictionary<string, Tetrimino> smallerDictionary)
        {
            var pickMe = _rng.Next(0, _candidates.Count - 1);
            var allKeys = smallerDictionary.Keys.ToList();
            var randomKey = allKeys[pickMe];
            return randomKey;
        }

        private Tetrimino NewTetrimino(string key, int x, int y)
        {
            var issued = ((Tetrimino)_candidates[key].Clone()).ShiftTo(x, y);
            _lastTetsArrayUpToTwelve.Add(issued);
            return issued;
        }
    }
}