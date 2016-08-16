using System;
using System.Collections.Generic;
using System.Linq;
using WinBlocks.Game.Model;

namespace WinBlocks.Tests
{
    public class BoardBuilderForTests
    {
        public Tuple<List<string>,Tetrimino> Populate(string pattern)
        {
            var lines = pattern.Trim().Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            var boardMap = new List<string>(lines);
            
            var leftMost = int.MaxValue;
            var topMost = int.MaxValue;
            var shapeLines = new List<string>();

            for (var y = 0; y < boardMap.Count; y++)
            {
                var row = boardMap[y];
                var shapeRow = "";
                for (var x = 0; x < row.Length; x++)
                {
                    var letter = row[x];

                    if (letter == 'C')
                    {
                        leftMost = x < leftMost ? x : leftMost;
                        topMost = y < topMost ? y : topMost;
                        shapeRow += letter;

                        row = row.Insert(x, ".");
                        row = row.Remove(x + 1, 1);
                    }
                }

                boardMap[y] = row;

                if (!string.IsNullOrWhiteSpace(shapeRow))
                {
                    shapeLines.Add(shapeRow);
                }
            }

            Tetrimino current = null;
            if (shapeLines.Any())
            {
                current = new Tetrimino(string.Join(Environment.NewLine, shapeLines), null, leftMost, topMost);
            }

            return new Tuple<List<string>, Tetrimino>(boardMap, current);
        }
    }
}