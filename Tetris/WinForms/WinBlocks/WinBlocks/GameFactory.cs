using WinBlocks.Game;
using WinBlocks.Game.BlockSelection;
using WinBlocks.Game.Model;
using WinBlocks.Game.Rendering;

namespace WinBlocks
{
    public static class GameFactory
    {
        public static Tetris NewGame(params IPostProcessContent[] postProcessors)
        {
            //var selector = new Selector(new Tetrominoes());
            var selector = new GrandmasterAceGenerator(new Tetrominoes());
            var game = new Tetris(selector);
            game.PostProcessors.AddRange(postProcessors);
            return game;
        }
    }
}