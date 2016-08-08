using WinBlocks.Game.Model;

namespace WinBlocks.Game.BlockSelection
{
    public interface ISelectBlocks
    {
        Tetrimino Random(int x = 0, int y = 0);
    }
}