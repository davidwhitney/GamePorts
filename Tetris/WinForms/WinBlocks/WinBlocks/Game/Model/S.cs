using System;

namespace WinBlocks.Game.Model
{
    public class S : Tetrimino
    {
        public S() : base(".SS" + Environment.NewLine +
                          "SS." + Environment.NewLine +
                          "...")
        {
        }
    }
}