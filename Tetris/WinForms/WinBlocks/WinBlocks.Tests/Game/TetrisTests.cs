using System.Linq;
using Moq;
using NUnit.Framework;
using WinBlocks.Game;
using WinBlocks.Game.BlockSelection;
using WinBlocks.Game.Input;
using WinBlocks.Game.Model;

namespace WinBlocks.Tests.Game
{
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
A...");
            
            _sut.Step();

            Assert.That(_sut.ToString(), Is.EqualTo(@"
....
....
....
A...".TrimStart()));
        }

        [Test]
        public void Step_ActiveMultiLinePiece_MovesTogether()
        {
            _sut.Current = new Tetrimino("A\r\nA", x: 0, y: 1);

            _sut.Step();

            Assert.That(_sut.ToString(), Is.EqualTo(@"
....
....
A...
A...".TrimStart()));
        }

        [Test]
        public void Move_AttemptToMoveOntoAnotherBlock_WontMove()
        {
            _sut.Current = new Tetrimino("B\r\nB", x: 0, y: 2);
           // _sut.BoardContents.Push(new Tetrimino("A\r\nA", x: 1, y: 2));

            _sut.Move(Direction.Right);

            var render = _sut.ToString().Trim();

            Assert.That(render, Is.EqualTo(@"
....
....
BA..
BA..".TrimStart()));
        }

        [Test]
        public void Step_PiecesColide_CollisionSticks()
        {
            _sut = NewGame(@"
....
.X..", lockPieces: true);
            
            _sut.Step(); // Spawn new
            _sut.Step();

            Assert.That(_sut.ToString(), Is.EqualTo(@"
.A..
.X..".TrimStart()));
        }

        [Test]
        public void Step_PiecesColide_CantStepPast()
        {
            _sut = NewGame(@"
....
.X..", lockPieces: true);
            
            _sut.Step(); // Spawn new
            _sut.Step();

            var render = _sut.ToString().Trim();

            Assert.That(render, Is.EqualTo(@"
.A..
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
A...", lockPieces: true);
            
            _sut.Step();

            var render = _sut.ToString().Trim();

            Assert.That(render, Is.EqualTo(@"
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
....
LLL.", lockPieces: true);

            _sut.Current = GenerateCurrentPieceAt(@"
....
....
...A
....");

            _sut.Step();
            _sut.Step(); // Line cleared as game "ticks"

            Assert.That(_sut.ToString(), Is.EqualTo(FourByFourBoard));

        }

        private Tetrimino GenerateCurrentPieceAt(string mask)
        {
            return NewGame(mask).Current;
        }

        private Tetris NewGame(string map = FourByFourBoard, bool lockPieces = false)
        {
            var state = new BoardBuilderForTests().Populate(map);
            var t = new Tetris(_selector.Object);
            t.Rows = state.Item1;
            t.Current = state.Item2;

            if (lockPieces)
            {
                t.Step();
            }

            return t;
        }

        private void NextSpawnIs(Tetrimino t)
        {
            _selector.Setup(x => x.Random(It.IsAny<int>(), It.IsAny<int>())).Returns(t);
        }
    }
}
