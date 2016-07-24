using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            _game.Step();
            textBox1.Clear();
            textBox1.Text = _game.ToString();
        }
    }
}
