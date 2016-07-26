using System;

namespace WinBlocks.Game.Model
{
    public class L : Tetrimino
    {
        public L() : base("..L" + Environment.NewLine +
                          "LLL" + Environment.NewLine +
                          "...")
        {
            RotationStates.Add(".L." + Environment.NewLine +
                               ".L." + Environment.NewLine +
                               ".LL");

            RotationStates.Add("..." + Environment.NewLine +
                               "LLL" + Environment.NewLine +
                               "L..");

            RotationStates.Add("LL." + Environment.NewLine +
                               ".L." + Environment.NewLine +
                               ".L.");
        }
    }
}