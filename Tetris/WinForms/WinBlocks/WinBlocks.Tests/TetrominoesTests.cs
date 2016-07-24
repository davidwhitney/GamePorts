using System.Linq;
using NUnit.Framework;

namespace WinBlocks.Tests
{
    public class TetrominoesTests
    {
        [TestCase("I")]
        [TestCase("O")]
        [TestCase("T")]
        [TestCase("S")]
        [TestCase("Z")]
        [TestCase("J")]
        [TestCase("L")]
        public void ContainsAllKeys(string key)
        {
            var valid = new Tetrominoes().Single(x => x.Id == key);

            Assert.That(valid, Is.Not.Null);
        }
    }
}