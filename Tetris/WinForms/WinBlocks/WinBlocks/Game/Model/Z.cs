using System;

namespace WinBlocks.Game.Model
{
    public class Z : Tetrimino
    {
        public Z() : base("ZZ." + Environment.NewLine +
                          ".ZZ" + Environment.NewLine +
                          "...")
        {
            RotationStates.Add("..Z" + Environment.NewLine +
                               ".ZZ" + Environment.NewLine +
                               ".Z");

            RotationStates.Add("..." + Environment.NewLine +
                               "ZZ." + Environment.NewLine +
                               ".ZZ");

            RotationStates.Add(".Z." + Environment.NewLine +
                               "ZZ." + Environment.NewLine +
                               "Z..");
        }
    }
}