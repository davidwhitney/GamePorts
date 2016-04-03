using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinBlocks
{
    public class Tetris
    {
        private readonly ISelectBlocks _selector;
        private List<string> _rows;
        private Tetrimino _current;

        public int Height => _rows.Count - 2;
        public int Width => _rows.First().Length;

        public Tetris(ISelectBlocks selector)
        {
            _selector = selector;
            _rows = new List<string>(Enumerable.Repeat("..........", 22));
        }

        public override string ToString()
        {
            var snapshot = new List<string>(_rows);
            
            if (_current != null)
            {
                var targetRowOffset = _current.Row;
                foreach (var patternLine in _current.PatternParts)
                {
                    snapshot[targetRowOffset] = snapshot[targetRowOffset].Insert(_current.Column, patternLine);
                    snapshot[targetRowOffset] = snapshot[targetRowOffset].Remove(_current.Column + patternLine.Length, patternLine.Length);

                    targetRowOffset++;
                }
            }

            var buffer = new StringBuilder();
            foreach (var row in snapshot.Skip(2))
            {
                buffer.AppendLine(row);
            }

            return buffer.ToString();
        }

        public void Spawn()
        {
            _current = _selector.Random();
            _current.Row = 0;
            _current.Column = 1;
        }

        public void Step()
        {
            _current.Row++;
        }
    }

    public class Tetrominoes : List<Tetrimino>
    {

        public Tetrominoes()
        {

            Add(new Tetrimino("IIII"));

            Add(new Tetrimino("OO" + Environment.NewLine +
                              "OO"));

            Add(new Tetrimino(".T." + Environment.NewLine +
                              "TTT"));

            Add(new Tetrimino(".SS" + Environment.NewLine +
                              "SS."));

            Add(new Tetrimino("ZZ." + Environment.NewLine +
                              ".ZZ"));

            Add(new Tetrimino("J.." + Environment.NewLine +
                              "JJJ"));

            Add(new Tetrimino("..L" + Environment.NewLine +
                              "LLL"));
        }
    }

    public interface ISelectBlocks
    {
        Tetrimino Random();
    }

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
            return _options[index];
        }
    }

    public class Tetrimino
    {
        public string Id => Pattern.First(c => c != '.').ToString();
        public string Pattern { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }

        public Tetrimino(string pattern)
        {
            Pattern = pattern;
        }

        public List<string> PatternParts => Pattern.Split(new[] {Environment.NewLine}, StringSplitOptions.None).ToList();
    }
}
