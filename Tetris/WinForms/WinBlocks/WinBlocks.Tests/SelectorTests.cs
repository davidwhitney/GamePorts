using NUnit.Framework;

namespace WinBlocks.Tests
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