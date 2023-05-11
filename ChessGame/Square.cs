using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChessGame
{
    class Square
    {
        static String chemin = "C:\\Users\\lukyc\\source\\repos\\ChessGame\\Image\\";
        public Piece piece;
        public bool blackSquare;
        public Rectangle rectangle;
        public readonly int row;
        public readonly int col;
        public Square(Piece p, Rectangle r, bool bS, int rw, int cl)
        {
            this.rectangle = r;
            this.piece = p;
            this.blackSquare = bS;
            this.row = rw;
            this.col = cl;
        }

        public void draw(Graphics g, bool selection)
        {
            Color backgroundSquare;
            Image img = Image.FromFile(chemin + piece.ToString() + ".png");
            if (blackSquare) backgroundSquare = Color.SlateGray; else backgroundSquare = Color.NavajoWhite;
            Brush b = new SolidBrush(backgroundSquare);
            Pen p = new Pen(b);
            
            if (!selection)
            {
                g.DrawRectangle(p, rectangle);
                g.FillRectangle(b, rectangle);

            }
            g.DrawImage(img, rectangle);
        }

        public void setPiece(Piece p)
        {
            this.piece = p;
        }
    }
}
