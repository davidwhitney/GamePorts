using System;
using System.Collections.Generic;
using System.Linq;
using WinBlocks.Game.Rendering;

namespace WinBlocks.Game.Model
{
    public class TetrisGrid : Grid<RenderLocation>, ICloneable
    {
        private const string EmptySpace = ".";

        public TetrisGrid(int width, int height) 
            : base(width, height, () => new RenderLocation {Content = EmptySpace })
        {
        }

        public TetrisGrid(List<string> grid)
            : base(1,1, () => new RenderLocation { Content = EmptySpace })
        {
            Rows = grid;
        }

        public List<string> Rows
        {
            get { return Storage.Select(row => string.Join("", row)).ToList(); }
            set
            {
                Height = value.Count;
                Width = value.Max(x => x.Length);
                Storage.Clear();

                for (var y = 0; y < value.Count; y++)
                {
                    var row = value[y];
                    var newRow = new List<RenderLocation>();
                    for (var x = 0; x < row.Length; x++)
                    {
                        var letter = row[x];
                        var content = new RenderLocation(x, y, letter.ToString());
                        newRow.Add(content);
                    }
                    Storage.Add(newRow);
                }
            }
        }

        public override bool IsOccupied(int x, int y)
        {
            if (!base.IsOccupied(x, y)) return false;
            return ValueAt(x, y).Content == EmptySpace;
        }

        public object Clone()
        {
            return new TetrisGrid(Width, Height)
            {
                Storage = InitiliseStorage(true)
            };
        }
    }
}