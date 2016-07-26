using System;
using System.Collections.Generic;
using System.Linq;
using WinBlocks.Game.Input;
using WinBlocks.Game.Rendering;

namespace WinBlocks.Game.Model
{
    public class Tetrimino
    {
        public string Id { get; }
        public string Pattern { get; private set; }

        public int Y { get; set; }
        public int X { get; set; }

        public List<string> RotationStates { get; }
        private int _currentState;

        public Tetrimino(string pattern)
        {
            Pattern = pattern;
            Id = Pattern.First(c => c != '.' && c != '\r' && c != '\n').ToString();
            RotationStates = new List<string> {Pattern};
            _currentState = 0;
        }

        public IEnumerable<RenderLocation> BlockLocations()
        {
            var patternParts = Pattern.Split(new[] {Environment.NewLine}, StringSplitOptions.None).ToList();

            for (var y = 0; y < patternParts.Count; y++)
            {
                var row = patternParts[y];

                for (var x = 0; x < row.Length; x++)
                {
                    var c = row[x];
                    if (c == '.')
                    {
                        continue;
                    }

                    yield return new RenderLocation
                    {
                        Content = c.ToString(),
                        X = X + x,
                        Y = Y + y
                    };
                }
            }
        }

        public void Rotate(Direction direction)
        {
            var next = PreviewRotation(direction);
            Pattern = next.Pattern;
        }

        public Tetrimino PreviewRotation(Direction direction)
        {
            var modifier = direction == Direction.Right ? +1 : -1;

            _currentState = _currentState + modifier >= RotationStates.Count ? 0 : _currentState + modifier;
            _currentState = _currentState < 0 ? RotationStates.Count - 1 : _currentState;

            var nextMap = RotationStates[_currentState];

            return new Tetrimino(nextMap)
            {
                X = X,
                Y = Y
            };
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Tetrimino);
        }

        private bool Equals(Tetrimino other)
        {
            return string.Equals(Pattern, other.Pattern) && Y == other.Y && X == other.X;
        }
    }
}