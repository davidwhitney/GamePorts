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

        public string Render(TetrisGrid board, Tetrimino activePiece)
        {
            var snapBoard = (TetrisGrid)board.Clone();
            if (activePiece != null)
            {
                foreach (var loc in activePiece.BlockLocations)
                {
                    snapBoard.SetValue(loc.X, loc.Y, loc);
                }
            }

            var snapshot = new List<string>();
            foreach (var row in snapBoard.RawRows)
            {
                var sbLine = new StringBuilder();

                foreach (var item in row)
                {
                    var token = item?.Content ?? ".";
                    token = PostProcessors.Aggregate(token, (current, processors) => processors.Process(current));
                    sbLine.Append(token);
                }

                snapshot.Add(sbLine.ToString());
            }

            return string.Join(Environment.NewLine, snapshot).Trim();
        }
    }
}