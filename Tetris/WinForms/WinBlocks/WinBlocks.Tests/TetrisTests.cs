using System.Linq;
using Moq;
using NUnit.Framework;

namespace WinBlocks.Tests
{
    [TestFixture]
    public class TetrisTests
    {
        private Tetris _sut;
        private Mock<ISelectBlocks> _selector;

        [SetUp]
        public void Setup()
        {
            _selector = new Mock<ISelectBlocks>();
            _selector.Setup(x => x.Random()).Returns(new Tetrimino("A"));
            _sut = new Tetris(_selector.Object);
        }

        [Test]
        public void Ctor_ProvidesEmptyBoard()
        {
            Assert.That(_sut.Width, Is.EqualTo(10));
            Assert.That(_sut.Height, Is.EqualTo(20));
        }

        [Test]
        public void ToString_RendersBoard()
        {
            var board = _sut.ToString();

            Assert.That(board.Trim(), Is.EqualTo(DefaultEmptyBoard));
        }

        [Test]
        public void Spawn_AddsRandomlySelectedBlockToBoard_ButOffScreen()
        {
            _sut.Step();

            Assert.That(_sut.ToString().Trim(), Is.EqualTo(DefaultEmptyBoard));
        }

        [Test]
        public void SpawnAndStepTwice_AddsRandomlySelectedBlockToBoard_PieceMovesIntoView()
        {
            _sut = new Tetris(_selector.Object, @"
....
....
....
....");

            StepOutOfHiddenZone();

            var render = _sut.ToString().Trim();

            Assert.That(render, Is.EqualTo(@"
.A..
....
....
....".TrimStart()));
        }

        [Test]
        public void SpawnAndStep_PieceIsTwoRowsBig_DrawsBoth()
        {
            _selector.Setup(x => x.Random()).Returns(new Tetrimino("AA\r\nAA"));
            _sut = new Tetris(_selector.Object, @"
....
....
....
....");

            StepOutOfHiddenZone();

            var render = _sut.ToString().Trim();

            Assert.That(render, Is.EqualTo(@"
.AA.
.AA.
....
....".TrimStart()));
        }

        [Test]
        public void SpawnAndStepAndStepPieceMovesDownBoard()
        {
            _sut = new Tetris(_selector.Object, @"
..........
..........
..........
..........");

            _sut.Step();
            StepOutOfHiddenZone();

            var render = _sut.ToString().Trim();

            Assert.That(render, Is.EqualTo(@"
..........
.A........
..........
..........".TrimStart()));
        }


        [Test]
        public void Step_CurrentCannotMove_Sticks()
        {
            _sut = new Tetris(_selector.Object, @"
....
....
....
A...");
            
            _sut.Step();

            var render = _sut.ToString().Trim();

            Assert.That(render, Is.EqualTo(@"
....
....
....
A...".TrimStart()));
        }

        [Test]
        public void Step_LastBlockSticks_NewBlockIntroduced()
        {
            _selector.Setup(x => x.Random()).Returns(new Tetrimino("A"));
            _sut = new Tetris(_selector.Object, @"
....
....
....
A...");
            
            _sut.Step();
            StepOutOfHiddenZone();

            var render = _sut.ToString().Trim();

            Assert.That(render, Is.EqualTo(@"
.A..
....
....
A...".TrimStart()));
        }

        private void StepOutOfHiddenZone()
        {
            _sut.Step();
            _sut.Step();
        }

        private static string DefaultEmptyBoard => @"
..........
..........
..........
..........
..........
..........
..........
..........
..........
..........
..........
..........
..........
..........
..........
..........
..........
..........
..........
..........".TrimStart();

    }

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
