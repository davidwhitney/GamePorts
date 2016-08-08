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

        [SetUp]
        public void Setup()
        {
            _selector = new Mock<ISelectBlocks>();
            NextSpawnIs(new Tetrimino("A", x: 1));
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

        [TestCase(Direction.Right, "..A.")]
        [TestCase(Direction.Left, "A...")]
        public void Move_GivenDirection_MovesPiece(Direction dir, string match)
        {
            _sut = NewGame(@"
....
....
....
....");

            _sut.Step(); // spawn
            _sut.Move(dir);

            var render = _sut.ToString().Trim();

            Assert.That(render.Take(4), Is.EqualTo(match));
        }

        [Test]
        public void SpawnAndStep_PieceIsTwoRowsBig_DrawsBoth()
        {
            NextSpawnIs(new Tetrimino("AA\r\nAA", x: 1));
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
            _sut.Current = new Tetrimino("A\r\nA", x: 0, y: 2);


            _sut.Step();

            var render = _sut.ToString().Trim();

            Assert.That(render, Is.EqualTo(@"
....
....
A...
A...".TrimStart()));
        }

        [Test]
        public void Move_AttemptToMoveOntoAnotherBlock_WontMove()
        {
            _sut = NewGame(@"
....
....
....
....");
            _sut.BoardContents.Push(new Tetrimino("A\r\nA", x: 1, y: 2));
            _sut.Current = new Tetrimino("A\r\nA", x: 0, y: 2);

            _sut.Move(Direction.Right);

            var render = _sut.ToString().Trim();

            Assert.That(render, Is.EqualTo(@"
....
....
AA..
AA..".TrimStart()));
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
            NextSpawnIs(new Tetrimino("A", x: 1));
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

        [Test]
        public void Step_BlockWouldCompleteTheLine_LineIsRemoved()
        {
            _sut = NewGame(@"
....
....
....
LLL.");
            var almostComplete = new Tetrimino("LLL", x: 0, y: 3);

            _sut.BoardContents.Push(almostComplete);

            _sut.Current = new Tetrimino("A", x: 3, y: 2);

            _sut.Step();
            _sut.Step();

            var render = _sut.ToString().Trim();

            //            Assert.That(render, Is.EqualTo(@"
            //....
            //....
            //....
            //LLLA".TrimStart()));  <-- THIS IS INCORRECT.

            Assert.That(render, Is.EqualTo(@"
....
....
....
....".TrimStart()));

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

        private void NextSpawnIs(Tetrimino t)
        {
            _selector.Setup(x => x.Random(It.IsAny<int>(), It.IsAny<int>())).Returns(t);
        }
    }
}
