using System;
using System.Collections.Generic;

namespace WinBlocks.Game
{
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
}