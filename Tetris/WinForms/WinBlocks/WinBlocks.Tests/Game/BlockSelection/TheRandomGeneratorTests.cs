using System.Collections.Generic;
using NUnit.Framework;
using WinBlocks.Game.BlockSelection;
using WinBlocks.Game.Model;

namespace WinBlocks.Tests.Game.BlockSelection
{
    [TestFixture]
    public class TheRandomGeneratorTests
    {
        private TheRandomGenerator _rg;
        private Tetrominoes _tets;

        [SetUp]
        public void SetUp()
        {
            _tets = new Tetrominoes();
            _rg = new TheRandomGenerator(_tets);
        }

        [Test]
        [Repeat(20)]
        public void DrawSevenPieces_GetOneOfEach()
        {
            var thingsIGot = new List<string>();

            for(var i = 0; i < 7; i++)
            {
                thingsIGot.Add(_rg.Random().Id);
            }

            Assert.That(thingsIGot.Contains("I"));
            Assert.That(thingsIGot.Contains("J"));
            Assert.That(thingsIGot.Contains("L"));
            Assert.That(thingsIGot.Contains("O"));
            Assert.That(thingsIGot.Contains("S"));
            Assert.That(thingsIGot.Contains("T"));
            Assert.That(thingsIGot.Contains("Z"));
        }
    }
}
