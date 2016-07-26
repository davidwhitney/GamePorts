using System;

namespace WinBlocks.Game.Model
{
    public class I : Tetrimino
    {
        public I() : base("...." + Environment.NewLine +
                          "IIII" + Environment.NewLine +
                          "...." + Environment.NewLine +
                          "....")
        {
            RotationStates.Add("..I." + Environment.NewLine +
                               "..I." + Environment.NewLine +
                               "..I." + Environment.NewLine +
                               "..I.");

            RotationStates.Add("...." + Environment.NewLine +
                               "...." + Environment.NewLine +
                               "IIII" + Environment.NewLine +
                               "....");

            RotationStates.Add(".I.." + Environment.NewLine +
                               ".I.." + Environment.NewLine +
                               ".I.." + Environment.NewLine +
                               ".I..");
        }
    }
}