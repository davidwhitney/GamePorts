using System;

namespace WinBlocks.Game.Model
{
    public class I : Tetrimino
    {
        public I() : base(@"IIII" + Environment.NewLine +
                          "...." + Environment.NewLine +
                          "....")
        {
        }
    }
}