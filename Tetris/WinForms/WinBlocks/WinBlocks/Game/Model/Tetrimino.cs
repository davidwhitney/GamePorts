using System;
using System.Collections.Generic;
using System.Linq;
using WinBlocks.Game.Input;
using WinBlocks.Game.Rendering;

namespace WinBlocks.Game.Model
{
    public class Tetrimino : ICloneable
    {
        private const char EmptySpace = '.';

        public string Id { get; }

        private string _pattern;
        public string Pattern
        {
            get { return _pattern; }
            private set { _pattern = value; BlockLocations = GenerateRenderLocations(); }
        }

        public Location Location { get; private set; } = new Location();

        public List<RenderLocation> BlockLocations { get; private set; }

        private List<string> PatternParts => Pattern.Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList();
        public List<string> RotationStates { get; set; }

        private int _currentState;

        public Tetrimino(string pattern, string[] rotationStates = null, int x = 0, int y = 0)
        {
            Pattern = pattern;
            ShiftTo(x, y);

            Id = Pattern.First(c => c != EmptySpace && c != '\r' && c != '\n').ToString();
            RotationStates = new List<string> {Pattern};

            if (rotationStates != null)
            {
                RotationStates.AddRange(rotationStates);
            }

            _currentState = 0;
        }

        public Tetrimino ShiftTo(int x, int y)
        {
            Location.X = x;
            Location.Y = y;
            BlockLocations = GenerateRenderLocations();
            return this;
        }

        public void Rotate(Direction direction)
        {
            Pattern = PreviewRotation(direction).Pattern;
        }

        public Tetrimino PreviewRotation(Direction direction)
        {
            var modifier = direction == Direction.Right ? +1 : -1;

            _currentState = _currentState + modifier >= RotationStates.Count ? 0 : _currentState + modifier;
            _currentState = _currentState < 0 ? RotationStates.Count - 1 : _currentState;

            var nextMap = RotationStates[_currentState];

            return new Tetrimino(nextMap)
            {
                Location = Location
            };
        }

        private List<RenderLocation> GenerateRenderLocations()
        {
            var locs = new List<RenderLocation>();
            var patternParts = PatternParts; // Snapshot
            for (var y = 0; y < patternParts.Count; y++)
            {
                var row = patternParts[y];

                for (var x = 0; x < row.Length; x++)
                {
                    var c = row[x];
                    if (c == EmptySpace)
                    {
                        continue;
                    }
                    locs.Add(new RenderLocation
                    {
                        Content = c.ToString(),
                        X = Location.X + x,
                        Y = Location.Y + y
                    });
                }
            }

            return locs;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Tetrimino);
        }

        public object Clone()
        {
            return new Tetrimino(
                Pattern,
                RotationStates.ToArray(),
                Location.X, Location.Y);
        }

        private bool Equals(Tetrimino other)
        {
            return string.Equals(Pattern, other.Pattern) 
                && Location.Y == other.Location.Y 
                && Location.X == other.Location.X;
        }
    }
}