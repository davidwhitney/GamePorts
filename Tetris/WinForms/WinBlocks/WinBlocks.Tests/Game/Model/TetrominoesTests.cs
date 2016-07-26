using System.Linq;
using NUnit.Framework;
using WinBlocks.Game.Model;

namespace WinBlocks.Tests.Game.Model
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
            var all = new Tetrominoes();

            var valid = all.Single(x => x.Id == key);

            Assert.That(valid, Is.Not.Null);
        }
    }
}