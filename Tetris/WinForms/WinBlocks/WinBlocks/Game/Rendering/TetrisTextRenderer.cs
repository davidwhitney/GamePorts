using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinBlocks.Game.Model;

namespace WinBlocks.Game.Rendering
{
    public class TetrisTextRenderer
    {
        public List<IPostProcessContent> PostProcessors = new List<IPostProcessContent>();

        public string Render(int width, int height, List<Tetrimino> contents)
        {
            var snapshot = new List<string>();
            
            var blocks = contents.SelectMany(x => x.BlockLocations).ToList();

            for (var y = 0; y < height; y++)
            {
                var sbLine = new StringBuilder();

                for (var x = 0; x < width; x++)
                {
                    var inLoc = blocks.SingleOrDefault(l => l.X == x && l.Y == y);
                    var token = inLoc != null ? inLoc.Content : ".";
                    token = PostProcessors.Aggregate(token, (current, processors) => processors.Process(current));

                    sbLine.Append(token);
                }

                snapshot.Add(sbLine.ToString());
            }

            return string.Join(Environment.NewLine, snapshot).Trim();
        }
    }
}