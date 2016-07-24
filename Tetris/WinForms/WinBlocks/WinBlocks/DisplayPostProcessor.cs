using WinBlocks.Game;

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