using WinBlocks.Game;
using WinBlocks.Game.Rendering;

namespace WinBlocks
{
    public class DisplayPostProcessor : IPostProcessContent
    {
        public string Process(string input)
        {
            if (input == ".")
            {
                return "░";
            }

            return "◙";
        }
    }
}