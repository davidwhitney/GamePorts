using System;
using System.Collections.Generic;
using System.Linq;

namespace WinBlocks
{
    /// <summary>
    /// Pretty much just used for scafolding test data.
    /// </summary>
    public class BoardBuilder
    {
        public Tuple<List<string>,Tetrimino> Populate(string pattern)
        {
            var lines = pattern.Trim().Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            var ourRows = new List<string>(lines);

            var leftMost = int.MaxValue;
            var topMost = 0;
            var shapeLines = new List<string>();

            for (var rowIndex = 0; rowIndex < ourRows.Count; rowIndex++)
            {
                var row = ourRows[rowIndex];
                var shapeRow = "";
                for (var index = 0; index < row.Length; index++)
                {
                    var letter = row[index];

                    if (letter != '.')
                    {
                        leftMost = index < leftMost ? index : leftMost;
                        topMost = rowIndex > topMost ? rowIndex : topMost;
                        shapeRow += letter;

                    }
                }
                if (!string.IsNullOrWhiteSpace(shapeRow))
                {
                    shapeLines.Add(shapeRow);
                }
            }

            Tetrimino current = null;
            if (shapeLines.Any())
            {
                current = new Tetrimino(string.Join(Environment.NewLine, shapeLines))
                {
                    X = leftMost,
                    Y = topMost
                };
            }

            return new Tuple<List<string>, Tetrimino>(ourRows, current);
        }
    }
}