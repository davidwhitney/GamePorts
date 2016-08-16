using System.Linq;
using Moq;
using NUnit.Framework;
using WinBlocks.Game;
using WinBlocks.Game.BlockSelection;
using WinBlocks.Game.Input;
using WinBlocks.Game.Model;

namespace WinBlocks.Tests.Game
{
    /// <summary>
    /// Use C's in the matrix here to set the current piece.
    /// </summary>
    [TestFixture]
    public class TetrisTests
    {
        private Tetris _sut;
        private Mock<ISelectBlocks> _selector;
        private const string FourByFourBoard = @"....
....
....
....";

        [SetUp]
        public void Setup()
        {
            _selector = new Mock<ISelectBlocks>();
            NextSpawnIs(new Tetrimino("A", x: 1));
            _sut = NewGame();
        }

        [Test]
        public void Step_EmptyBoard_AddsRandomlySelectedBlockToBoard()
        {
            NextSpawnIs(new Tetrimino("A", x: 1));

            _sut.Step();

            Assert.That(_sut.ToString(), Is.EqualTo(@"
.A..
....
....
....".TrimStart()));
        }

        [TestCase(Direction.Right, "..A.")]
        [TestCase(Direction.Left, "A...")]
        public void Move_GivenDirection_MovesPiece(Direction dir, string match)
        {
            _sut.Step(); // spawn

            _sut.Move(dir);

            Assert.That(_sut.ToString().Take(4), Is.EqualTo(match));
        }

        [Test]
        public void Step_PieceIsTwoRowsBig_DrawsBoth()
        {
            NextSpawnIs(new Tetrimino("AA\r\nAA", x: 1));

            _sut.Step();

            Assert.That(_sut.ToString(), Is.EqualTo(@"
.AA.
.AA.
....
....".TrimStart()));
        }

        [Test]
        public void StepTwice_EmptyBoard_PieceIsSpawnedAndMoved()
        {
            _sut.Step();
            _sut.Step();

            Assert.That(_sut.ToString(), Is.EqualTo(@"
....
.A..
....
....".TrimStart()));
        }

        [Test]
        public void Step_CurrentCannotMove_Sticks()
        {
            _sut = NewGame(@"
....
....
....
C...");
            
            _sut.Step();

            Assert.That(_sut.ToString(), Is.EqualTo(@"
....
....
....
C...".TrimStart()));
        }

        [Test]
        public void Step_ActiveMultiLinePiece_MovesTogether()
        {
            _sut = NewGame(@"
....
C...
C...
....");

            _sut.Step();

            Assert.That(_sut.ToString(), Is.EqualTo(@"
....
....
C...
C...".TrimStart()));
        }

        [Test]
        public void Move_AttemptToMoveOntoAnotherBlock_WontMove()
        {
            _sut = NewGame(@"
....
....
CA..
CA..");

            _sut.Move(Direction.Right);

            Assert.That(_sut.ToString(), Is.EqualTo(@"
....
....
CA..
CA..".TrimStart()));
        }

        [Test]
        public void Step_PiecesColide_CollisionSticks()
        {
            _sut = NewGame(@"
....
.X..");
            
            _sut.Step(); // Spawns new
            _sut.Step(); // Stays in place

            Assert.That(_sut.ToString(), Is.EqualTo(@"
.A..
.X..".TrimStart()));
        }

        [Test]
        public void Step_PiecesColide_CantStepPast()
        {
            _sut = NewGame(@"
.C..
.X..");
            
            _sut.Step();

            Assert.That(_sut.ToString(), Is.EqualTo(@"
.C..
.X..".TrimStart()));
        }

        [Test]
        public void Step_LastBlockSticks_NewBlockIntroduced()
        {
            NextSpawnIs(new Tetrimino("A", x: 1));
            _sut = NewGame(@"
....
....
....
A...");
            
            _sut.Step();

            Assert.That(_sut.ToString(), Is.EqualTo(@"
.A..
....
....
A...".TrimStart()));
        }

        [Test]
        public void Step_BlockWouldCompleteTheLine_LineIsRemoved()
        {
            _sut = NewGame(@"
....
....
...C
AAA.");

            _sut.Step();
            _sut.Step(); // Line cleared as game "ticks"

            Assert.That(_sut.ToString(), Is.EqualTo(@"
....
....
....
....".Trim()));
        }

        [Test]
        public void Step_BlockWouldCompleteTheLine_LinesDropDown()
        {
            _sut = NewGame(@"
....
....
B..C
AAA.");

            _sut.Step();
            _sut.Step(); // Line cleared as game "ticks"

            Assert.That(_sut.ToString(), Is.EqualTo(@"
....
....
....
B...".Trim()));
        }

        private Tetris NewGame(string map = FourByFourBoard)
        {
            var state = new BoardBuilderForTests().Populate(map);
            var t = new Tetris(_selector.Object)
            {
                Rows = state.Item1,
                Current = state.Item2
            };
            
            return t;
        }

        private void NextSpawnIs(Tetrimino t)
        {
            _selector.Setup(x => x.Random(It.IsAny<int>(), It.IsAny<int>())).Returns(t);
        }
    }
}
