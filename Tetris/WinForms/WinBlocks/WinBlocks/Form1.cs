using System;
using System.Windows.Forms;
using WinBlocks.Game;
using WinBlocks.Game.Input;
using Timer = System.Windows.Forms.Timer;

namespace WinBlocks
{
    public partial class Form1 : Form
    {
        private Tetris _game;

        public Form1()
        {
            InitializeComponent();
            _game = GameFactory.NewGame(new DisplayPostProcessor());
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            _game = GameFactory.NewGame(new DisplayPostProcessor());
        }

        private void btnStep_Click(object sender, EventArgs e)
        {
            Step();
        }

        private void UiMove(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Right)
            {
                _game.Move(Direction.Right);
            }
            if (e.KeyData == Keys.Left)
            {
                _game.Move(Direction.Left);
            }
            if (e.KeyData == Keys.Down)
            {
                _game.Move(Direction.Down);
            }
            Draw();
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            var timer = new Timer {Interval = 800};
            timer.Tick += (sender1, e1) => { Step(); };
            timer.Start();
        }

        private void Step()
        {
            _game.Step();
            Draw();
        }

        private void Draw()
        {
            textBox1.Clear();
            textBox1.Text = _game.ToString();
        }
    }
}