using WinBlocks.Game;

namespace WinBlocks
{
    public static class GameFactory
    {
        public static Tetris NewGame(params IPostProcessContent[] postProcessors)
        {
            var selector = new Selector(new Tetrominoes());
            var game = new Tetris(selector);
            game.PostProcessors.AddRange(postProcessors);
            return game;
        }
    }
}