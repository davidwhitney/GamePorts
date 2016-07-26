using System;

namespace WinBlocks.Game.Model
{
    public class O : Tetrimino
    {
        public O() : base(".OO." + Environment.NewLine +
                          ".OO." + Environment.NewLine +
                          "....")
        {
        }
    }
}