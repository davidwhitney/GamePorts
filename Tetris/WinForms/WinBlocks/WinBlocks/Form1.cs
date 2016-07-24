using System;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace WinBlocks
{
    public partial class Form1 : Form
    {
        private Tetris _game;
        private Selector _selector;

        public Form1()
        {
            InitializeComponent();
            NewGame();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            NewGame();
        }

        private void btnStep_Click(object sender, EventArgs e)
        {
            Step();
        }
        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            MovePiece(e);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            MovePiece(e);
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            var timer = new Timer {Interval = 800};
            timer.Tick += (sender1, e1) => { Step(); };
            timer.Start();
        }

        private void NewGame()
        {
            _selector = new Selector(new Tetrominoes());
            _game = new Tetris(_selector);
            _game.PostProcessors.Add(new DisplayPostProcessor());
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

        private void MovePiece(KeyEventArgs e)
        {
            if (e.KeyData == Keys.Right)
            {
                _game.Move(Direction.Right);
            }
            if (e.KeyData == Keys.Left)
            {
                _game.Move(Direction.Left);
            }
            Draw();
        }
    }
}
