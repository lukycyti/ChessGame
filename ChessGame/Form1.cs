using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChessGame
{
    partial class Form1 : Form
    {
        Boolean black;
        public Piece p;

        public Form1(Boolean black)
        {
            InitializeComponent();
            this.black = black;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (black) p = Piece.BLACK_ROOK;
            else p = Piece.WHITE_ROOK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (black) p = Piece.BLACK_KNIGHT;
            else p = Piece.WHITE_KNIGHT;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (black) p = Piece.BLACK_BISHOP;
            else p = Piece.WHITE_BISHOP;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (black) p = Piece.BLACK_QUEEN;
            else p = Piece.WHITE_QUEEN;
        }
    }
}
