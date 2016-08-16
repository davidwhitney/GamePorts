using System;
using System.Collections.Generic;
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
        }

        private void UiMove(object sender, KeyEventArgs e)
        {
            new Dictionary<Keys, Action>
            {
                {Keys.Right, () => _game.Move(Direction.Right)},
                {Keys.Left, () => _game.Move(Direction.Left)},
                {Keys.Down, () => _game.Move(Direction.Down)},
                {Keys.Z, () => _game.Rotate(Direction.Left)},
                {Keys.X, () => _game.Rotate(Direction.Right)},
            }[e.KeyData]();
            Draw();
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            _game = GameFactory.NewGame(new DisplayPostProcessor());
            var timer = new Timer {Interval = 800};
            timer.Tick += (sender1, e1) =>
            {
                _game.Step();
                Draw();
            };
            timer.Start();
        }

        private void Draw()
        {
            textBox1.Clear();
            textBox1.Text = _game.ToString();
        }
    }
}