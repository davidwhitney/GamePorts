using System;

namespace WinBlocks.Game.Model
{
    public class T : Tetrimino
    {
        public T() : base(".T." + Environment.NewLine +
                          "TTT" + Environment.NewLine +
                          "...")
        {
            RotationStates.Add(".T." + Environment.NewLine +
                               ".TT" + Environment.NewLine +
                               ".T.");

            RotationStates.Add("..." + Environment.NewLine +
                               "TTT" + Environment.NewLine +
                               ".T.");

            RotationStates.Add(".T." + Environment.NewLine +
                               "TT." + Environment.NewLine +
                               ".T.");
        }
    }
}