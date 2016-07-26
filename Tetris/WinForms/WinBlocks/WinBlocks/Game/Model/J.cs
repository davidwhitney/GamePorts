using System;

namespace WinBlocks.Game.Model
{
    public class J : Tetrimino
    {
        public J() : base("J.." + Environment.NewLine +
                          "JJJ" + Environment.NewLine +
                          "...")
        {
            RotationStates.Add(".JJ" + Environment.NewLine +
                               ".J." + Environment.NewLine +
                               ".J.");

            RotationStates.Add("..." + Environment.NewLine +
                               "JJJ" + Environment.NewLine +
                               "..J");

            RotationStates.Add(".J." + Environment.NewLine +
                               ".J." + Environment.NewLine +
                               "JJ.");
        }
    }
}