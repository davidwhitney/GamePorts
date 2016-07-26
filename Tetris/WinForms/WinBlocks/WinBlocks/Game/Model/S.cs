using System;

namespace WinBlocks.Game.Model
{
    public class S : Tetrimino
    {
        public S() : base(".SS" + Environment.NewLine +
                          "SS." + Environment.NewLine +
                          "...")
        {
            RotationStates.Add(".S." + Environment.NewLine +
                               ".SS" + Environment.NewLine +
                               "..S");

            RotationStates.Add("..." + Environment.NewLine +
                               ".SS" + Environment.NewLine +
                               "SS.");

            RotationStates.Add("S.." + Environment.NewLine +
                               "SS." + Environment.NewLine +
                               ".S.");
        }
    }
}