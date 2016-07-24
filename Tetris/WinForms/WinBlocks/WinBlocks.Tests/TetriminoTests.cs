using System;
using System.Linq;
using NUnit.Framework;
using WinBlocks.Game;

namespace WinBlocks.Tests
{
    [TestFixture]
    public class TetriminoTests
    {
        [Test]
        public void RenderLocations_SingleLine_ReturnsCorrectCoordsForRendering()
        {
            var tet = new Tetrimino("II")
            {
                Y = 0,
                X = 0
            };

            var locs = tet.BlockLocations().ToList();

            Assert.That(locs[0], Is.EqualTo(new RenderLocation(0, 0, "I")));
            Assert.That(locs[1], Is.EqualTo(new RenderLocation(1, 0, "I")));
        }

        [Test]
        public void RenderLocations_SingleLineWithSpace_ReturnsOnlyRequiredElements()
        {
            var tet = new Tetrimino("I.I")
            {
                Y = 0,
                X = 0
            };

            var locs = tet.BlockLocations().ToList();

            Assert.That(locs[0], Is.EqualTo(new RenderLocation(0, 0, "I")));
            Assert.That(locs[1], Is.EqualTo(new RenderLocation(2, 0, "I")));
        }

        [Test]
        public void RenderLocations_MultiLine_ReturnsCorrectCoordsForRendering()
        {
            var tet = new Tetrimino("II" + Environment.NewLine + "II")
            {
                Y = 0,
                X = 0
            };

            var locs = tet.BlockLocations().ToList();

            Assert.That(locs[0], Is.EqualTo(new RenderLocation(0, 0, "I")));
            Assert.That(locs[1], Is.EqualTo(new RenderLocation(1, 0, "I")));
            Assert.That(locs[2], Is.EqualTo(new RenderLocation(0, 1, "I")));
            Assert.That(locs[3], Is.EqualTo(new RenderLocation(1, 1, "I")));
        }
    }
}