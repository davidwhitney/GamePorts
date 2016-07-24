using System;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace WinBlocks
{
    public partial class Form1 : Form
    {
        private Tetris _game;
        private readonly Selector _selector;

        public Form1()
        {
            InitializeComponent();

            _selector = new Selector(new Tetrominoes());
            _game = new Tetris(_selector);
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            _game = new Tetris(_selector);
        }

        private void btnStep_Click(object sender, EventArgs e)
        {
            Step();
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
            textBox1.Clear();
            textBox1.Text = _game.ToString();
        }
    }
}
