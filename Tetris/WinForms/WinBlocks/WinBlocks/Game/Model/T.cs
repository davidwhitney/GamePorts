using System;

namespace WinBlocks.Game.Model
{
    public class T : Tetrimino
    {
        public T() : base(".T." + Environment.NewLine +
                          "TTT" + Environment.NewLine +
                          "...")
        {
        }
    }
}