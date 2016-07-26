using System;

namespace WinBlocks.Game.Model
{
    public class Z : Tetrimino
    {
        public Z() : base("ZZ." + Environment.NewLine +
                          ".ZZ" + Environment.NewLine +
                          "...")
        {
        }
    }
}