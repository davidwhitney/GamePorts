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

            Assert.That(board.Trim(), Is.EqualTo(Tetris.EmptyBoard));
        }
        
        [Test]
        public void SpawnAndStepTwice_AddsRandomlySelectedBlockToBoard_PieceMovesIntoView()
        {
            _sut = NewGame(@"
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
            _sut = NewGame(@"
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
            _sut = NewGame(@"
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
            _sut = NewGame(@"
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
        public void Step_CurrentCannotMoveAndIsMoreThanOneLineBig_Sticks()
        {
            _sut = NewGame(@"
....
....
....
....");
            _sut.Current = new Tetrimino("A\r\nA") {X = 0, Y = 2};


            _sut.Step();

            var render = _sut.ToString().Trim();

            Assert.That(render, Is.EqualTo(@"
....
....
A...
A...".TrimStart()));
        }

        [Test]
        public void Step_PiecesColide_CollisionSticks()
        {
            _sut = NewGame(@"
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
            _sut = NewGame(@"
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
            _sut = NewGame(@"
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

        private Tetris NewGame(string map)
        {
            var state = new BoardBuilderForTests().Populate(map);
            return new Tetris(_selector.Object)
            {
                Rows = state.Item1,
                Current = state.Item2
            };
        }
    }
}
