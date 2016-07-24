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
            Assert.That(_sut.Height, Is.EqualTo(22));
        }

        [Test]
        public void ToString_RendersBoard()
        {
            var board = _sut.ToString();

            Assert.That(board.Trim(), Is.EqualTo(DefaultEmptyBoard));
        }
        
        [Test]
        public void SpawnAndStepTwice_AddsRandomlySelectedBlockToBoard_PieceMovesIntoView()
        {
            _sut = new Tetris(_selector.Object, @"
....
....
....
....");

            _sut.Step();

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

            _sut.Step();

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
            _sut.Step();

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
        public void Step_PiecesColide_CollisionSticks()
        {
            _sut = new Tetris(_selector.Object, @"
....
.X..");
            
            _sut.Step();
            _sut.Step();

            var render = _sut.ToString().Trim();

            Assert.That(render, Is.EqualTo(@"
.A..
.X..".TrimStart()));
        }

        [Test]
        public void Step_PiecesColide_CantStepPast()
        {
            _sut = new Tetris(_selector.Object, @"
....
.X..");
            
            _sut.Step();
            _sut.Step();
            _sut.Step();

            var render = _sut.ToString().Trim();

            Assert.That(render, Is.EqualTo(@"
.A..
.X..".TrimStart()));
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
            
            _sut.Step(); // Because we're loading from pattern, this step will clear the current block variable
            _sut.Step();

            var render = _sut.ToString().Trim();

            Assert.That(render, Is.EqualTo(@"
.A..
....
....
A...".TrimStart()));
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
..........
..........
..........".TrimStart();

    }
}
