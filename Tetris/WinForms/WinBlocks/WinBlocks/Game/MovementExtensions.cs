using WinBlocks.Game.Input;
using WinBlocks.Game.Model;

namespace WinBlocks.Game
{
    public static class MovementExtensions
    {
        public static Delta ToDelta(this Direction direction)
        {
            var delta = new Delta();

            if (direction == Direction.Right)
            {
                delta.X = 1;
            }

            if (direction == Direction.Left)
            {
                delta.X = -1;
            }

            if (direction == Direction.Down)
            {
                delta.Y = 1;
            }

            return delta;
        }
    }
}