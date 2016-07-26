using System;
using System.Linq;
using NUnit.Framework;
using WinBlocks.Game.Input;
using WinBlocks.Game.Model;
using WinBlocks.Game.Rendering;

namespace WinBlocks.Tests.Game.Model
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

        [Test]
        public void PreviewRotation_GivenDirection_ReturnsRotatedState()
        {
            var tet = new Tetrimino("II\r\nI.");
            tet.RotationStates.Add("II\r\n.I");

            var nextState = tet.PreviewRotation(Direction.Right);

            Assert.That(nextState.Pattern, Is.EqualTo("II\r\n.I"));
        }

        [Test]
        public void Rotate_GivenDirection_ChangesState()
        {
            var tet = new Tetrimino("II\r\nI.");
            tet.RotationStates.Add("II\r\n.I");

            tet.Rotate(Direction.Right);

            Assert.That(tet.Pattern, Is.EqualTo("II\r\n.I"));
        }

        [Test]
        public void Rotate_RotateAndBack_CanRotateBack()
        {
            var tet = new Tetrimino("II\r\nI.");
            tet.RotationStates.Add("II\r\n.I");

            tet.Rotate(Direction.Right);
            tet.Rotate(Direction.Left);

            Assert.That(tet.Pattern, Is.EqualTo("II\r\nI."));
        }

        [Test]
        public void Rotate_RotateAround_WrapsRotation()
        {
            var tet = new Tetrimino("II\r\nI.");
            tet.RotationStates.Add("II\r\n.I");

            tet.Rotate(Direction.Left);

            Assert.That(tet.Pattern, Is.EqualTo("II\r\n.I"));
        }

        [Test]
        public void Rotate_ModifiedLocationOfParts()
        {
            var tet = new Tetrimino("II\r\n..");
            tet.RotationStates.Add(".I\r\n.I");

            var parts1 = tet.PatternParts.ToList();
            var cells1 = tet.BlockLocations().ToList();
            tet.Rotate(Direction.Left);

            var parts2 = tet.PatternParts.ToList();
            var cells2 = tet.BlockLocations().ToList();

            Assert.That(parts1, Is.Not.EquivalentTo(parts2));
            Assert.That(cells1, Is.Not.EquivalentTo(cells2));
        }
    }
}