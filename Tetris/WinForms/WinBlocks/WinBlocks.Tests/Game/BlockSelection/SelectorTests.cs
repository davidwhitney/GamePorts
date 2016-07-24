using NUnit.Framework;
using WinBlocks.Game.BlockSelection;
using WinBlocks.Game.Model;

namespace WinBlocks.Tests.Game.BlockSelection
{
    public class SelectorTests
    {
        private Tetrominoes _tets;
        private Selector _sel;

        [SetUp]
        public void Setup()
        {
            _tets = new Tetrominoes();
            _sel = new Selector(_tets);
        }

        [Test]
        public void Random_SelectsRandomBlock()
        {
            var block = _sel.Random();

            Assert.That(block, Is.Not.Null);
            Assert.That(_tets, Does.Contain(block));
        }
    }
}