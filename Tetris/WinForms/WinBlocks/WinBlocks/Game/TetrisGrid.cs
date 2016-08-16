using System.Collections.Generic;
using System.Linq;
using WinBlocks.Game.Rendering;

namespace WinBlocks.Game
{
    public class TetrisGrid : Grid<RenderLocation>
    {
        public TetrisGrid(int width, int height) : base(width, height)
        {
        }

        public List<string> Rows
        {
            get { return Storage.Select(row => string.Join<RenderLocation>("", row)).ToList(); }
            set
            {
                Height = value.Count;
                Width = value.Max(x => x.Length);
                Storage.Clear();
                for (int y = 0; y < value.Count; y++)
                {
                    var row = value[y];
                    var newRow = new List<RenderLocation>();
                    for (int x = 0; x < row.Length; x++)
                    {
                        var letter = row[x];
                        var content = new RenderLocation(x, y, letter.ToString());
                        newRow.Add(content);
                    }
                    Storage.Add(newRow);
                }
            }
        }
    }
}