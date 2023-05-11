using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ChessGame
{
    public partial class Board : Form
    {
        Square[,] squares = new Square[8, 8];
        Square selection;
        Square BlackKing, WhiteKing;
        HashSet<Square> PlayablePosSelect = new HashSet<Square>();
        Dictionary<Square, HashSet<Square>> PlayablePos = new Dictionary<Square, HashSet<Square>>();
        Boolean blackTurn = false;
        int nbTurn = 1;
        bool check = false;

        public Board()
        {
            InitializeComponent();
            initializeSquare();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void initializeSquare()
        {
            bool blackSquare = false;
            int start = 80;
            Size size = new Size(100, 100);
            for (int i = 0; i < 8; i++)
            {
                if (blackSquare) blackSquare = false; else blackSquare = true;
                for (int j = 0; j < 8; j++)
                {
                    Rectangle r = new Rectangle(new Point(i * 100 + start, j * 100 + start), size);
                    squares[j, i] = new Square(GetStartingPiece(j, i), r, blackSquare, i , j);
                    Square actual = squares[j, i];
                    if (actual.piece == Piece.BLACK_KING) BlackKing = actual;
                    if (actual.piece == Piece.WHITE_KING) WhiteKing = actual;
                    if (blackSquare) blackSquare = false; else blackSquare = true;
                    PlayablePos.Add(actual, new HashSet<Square>());
                }
            }
            actualizePlayablePos();
            Turn.Text = nbTurn.ToString();
        }

        private bool IsBlack(Piece p)
        {
            return p == Piece.BLACK_KING || p == Piece.BLACK_QUEEN || p == Piece.BLACK_ROOK
                || p == Piece.BLACK_BISHOP || p == Piece.BLACK_KNIGHT || p == Piece.BLACK_PAWN;
        }

        private void ChangePlayer()
        {
            blackTurn = blackTurn != true;
            
        }

        private bool displacePawn(Square sq)
        {
            if (selection.piece == Piece.EMPTY) return false;
            else
            {
                
                if (selection.piece == Piece.BLACK_KING) BlackKing = sq;
                if (selection.piece == Piece.WHITE_KING) WhiteKing = sq;
                sq.setPiece(selection.piece);
                selection.setPiece(Piece.EMPTY);
                if (sq.piece == Piece.BLACK_PAWN && sq.col == 7
                    || sq.piece == Piece.WHITE_PAWN && sq.col == 0) {
                    Form1 form = new Form1(blackTurn);
                    if(form.ShowDialog() == DialogResult.OK) { 
                        sq.piece = form.p;
                        form.Close();
                    }
                    else
                    {
                        if (blackTurn) sq.piece = Piece.BLACK_ROOK;
                        else sq.piece = Piece.WHITE_ROOK;
                    }
                }
            }
            return true;
        }

        private bool MoveCauseCheck(Square newSq)
        {
            bool causeCheck = false;
            Piece beaten = newSq.piece;
            newSq.setPiece(selection.piece);
            selection.setPiece(Piece.EMPTY);
            if (newSq.piece == Piece.BLACK_KING) BlackKing = newSq;
            if (newSq.piece == Piece.WHITE_KING) WhiteKing = newSq;
            if (isCheck(blackTurn))
            {
                causeCheck = true;
            }
            selection.setPiece(newSq.piece);
            newSq.setPiece(beaten);
            if (selection.piece == Piece.BLACK_KING) BlackKing = selection;
            if (selection.piece == Piece.WHITE_KING) WhiteKing = selection;
            return causeCheck;
        }

        public bool isCheck(bool blackKing)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Square sq = squares[i, j];
                    if (sq.piece == Piece.EMPTY || IsBlack(sq.piece) == blackKing) continue;
                    HashSet<Square> play = playablePos(sq);
                    if (blackKing && play.Contains(this.BlackKing)
                        || !blackKing && play.Contains(this.WhiteKing)) 
                        return true;
                }
            }
            
            return false;
        }

        public int isCheckMateOrDraw(bool blackKing)
        {
            
            foreach(Square s in PlayablePos.Keys)
            {
                HashSet<Square> pos;
                if (s.piece == Piece.EMPTY || IsBlack(s.piece) == blackKing) continue;
                PlayablePos.TryGetValue(s, out pos);
                if (pos.Count != 0) return 0;

            }
            if (isCheck(!blackKing)) return 1;
            else return 2;
        }

        private bool DifferentColor(Piece p, Piece p2)
        {
            if (p == Piece.EMPTY || p2 == Piece.EMPTY) return false;
            return IsBlack(p) && !IsBlack(p2) || !IsBlack(p) && IsBlack(p2);
        }

        private Piece GetStartingPiece(int row, int col)
        {
            Boolean black;
            if (row > 1 && row < 6) return Piece.EMPTY;
            if (row == 1) return Piece.BLACK_PAWN;
            if (row == 6) return Piece.WHITE_PAWN;
            black = row < 2;
            switch (col)
            {
                case 0:
                case 7:
                    if (black) return Piece.BLACK_ROOK; else return Piece.WHITE_ROOK;
                case 1:
                case 6:
                    if (black) return Piece.BLACK_KNIGHT; else return Piece.WHITE_KNIGHT;
                case 2:
                case 5:
                    if (black) return Piece.BLACK_BISHOP; else return Piece.WHITE_BISHOP;
                case 3:
                    if (black) return Piece.BLACK_QUEEN; else return Piece.WHITE_QUEEN;
                case 4:
                    if (black) return Piece.BLACK_KING; else return Piece.WHITE_KING;
            }
            return Piece.EMPTY;
        }

        HashSet<Square> playablePos(Square sq)
        {
            Piece p = sq.piece;
            switch (p)
            {
                case Piece.BLACK_ROOK:
                case Piece.WHITE_ROOK:
                    return playRook(sq);
                case Piece.BLACK_KNIGHT:
                case Piece.WHITE_KNIGHT:
                    return playKnight(sq);
                case Piece.BLACK_BISHOP:
                case Piece.WHITE_BISHOP:
                    return playBishop(sq);
                case Piece.BLACK_QUEEN:
                case Piece.WHITE_QUEEN:
                    return playQueen(sq);
                case Piece.BLACK_KING:
                case Piece.WHITE_KING:
                    return playKing(sq);
                case Piece.BLACK_PAWN:
                case Piece.WHITE_PAWN:
                    return playPawn(sq);
            }
            return PlayablePosSelect;
        }

        private Square nextPos(Square sq, Direction d)
        {
            switch (d)
            {
                case Direction.NORTH:
                    return GetSquare(sq, -1, 0);
                case Direction.SOUTH:
                    return GetSquare(sq, 1, 0);
                case Direction.EST:
                    return GetSquare(sq, 0, 1);
                case Direction.WEST:
                    return GetSquare(sq, 0, -1);
                case Direction.NORTH_WEST:
                    return GetSquare(sq, -1, -1);
                case Direction.NORTH_EST:
                    return GetSquare(sq, -1, 1);
                case Direction.SOUTH_EST:
                    return GetSquare(sq, 1, 1);
                case Direction.SOUTH_WEST:
                    return GetSquare(sq, 1, -1);
                default:
                    return null;
            }
        }

        private HashSet<Square> playRook(Square sq)
        {
            HashSet<Square> sqs = new HashSet<Square>();
            foreach (Direction d in Cardinal())
            {
                Square next = nextPos(sq, d);
                bool stop = false;
                while (inBoard(next) && !stop)
                {
                    if (next.piece != Piece.EMPTY)
                    {
                        if (DifferentColor(next.piece, sq.piece))
                        {
                            sqs.Add(next);
                            next = nextPos(next, d);
                        }
                        stop = true;
                    }
                    else
                    {
                        sqs.Add(next);
                        next = nextPos(next, d);
                    }
                }
            };
            return sqs;
        }

        private HashSet<Square> playBishop(Square sq)
        {

            HashSet<Square> sqs = new HashSet<Square>();
            foreach (Direction d in Diagonal())
            {
                Square next = nextPos(sq, d);
                bool stop = false;
                while (inBoard(next) && !stop)
                {
                    if (next.piece != Piece.EMPTY)
                    {
                        if (DifferentColor(next.piece, sq.piece))
                        {
                            sqs.Add(next);
                            next = nextPos(next, d);
                        }
                        stop = true;
                    }
                    else
                    {
                        sqs.Add(next);
                        next = nextPos(next, d);
                    }
                }
            };
            return sqs;
        }

        private HashSet<Square> playKnight(Square sq)
        {
            HashSet<Square> p = new HashSet<Square>();
            foreach(Direction d in Cardinal()) {
                Square next = nextPos(sq, d);
                if (inBoard(next))
                {
                   foreach(Direction dir in DiagonalDirection(d))  {
                        Square nextDiag = nextPos(next, dir);
                        if (inBoard(nextDiag))
                        {
                            if (DifferentColor(nextDiag.piece, sq.piece) || nextDiag.piece == Piece.EMPTY)
                            {
                                p.Add(nextDiag);
                            }
                        }
                    };
                }
            };
            return p;
        }

        private HashSet<Square> playQueen(Square sq)
        {

            HashSet<Square> sqs = new HashSet<Square>();
            foreach (Direction d in AllDirection())
            {
                Square next = nextPos(sq, d);
                bool stop = false;
                while (inBoard(next) && !stop)
                {
                    if (next.piece != Piece.EMPTY)
                    {
                        if (DifferentColor(next.piece, sq.piece))
                        {
                            sqs.Add(next);
                            next = nextPos(next, d);
                        }
                        stop = true;
                    }
                    else
                    {
                        sqs.Add(next);
                        next = nextPos(next, d);
                    }
                }
            };
            return sqs;
        }

        private HashSet<Square> playKing(Square sq)
        {
            HashSet<Square> sqs = new HashSet<Square>();
            foreach (Direction d in AllDirection())
            {
                Square next = nextPos(sq, d);
                if(inBoard(next))
                {
                    if(next.piece == Piece.EMPTY || DifferentColor(next.piece, sq.piece))
                    {
                        sqs.Add(next);
                    }
                }
            };
            return sqs;
        
        }

        private HashSet<Square> playPawn(Square sq)
        {
            
            HashSet<Square> p = new HashSet<Square>();
            Direction depl;
            HashSet<Direction> att = new HashSet<Direction>();
            if (IsBlack(sq.piece))
            {
                depl = Direction.SOUTH; att.Add(Direction.SOUTH_EST); att.Add(Direction.SOUTH_WEST);
            }
            else
            {
                depl = Direction.NORTH; att.Add(Direction.NORTH_EST); att.Add(Direction.NORTH_WEST);
            }
            Square next = nextPos(sq, depl);
            if (inBoard(next) && next.piece == Piece.EMPTY) { p.Add(next);  }
            // Deplacement de 2 au début
            if (sq.col == 6 && next.piece == Piece.EMPTY && !IsBlack(sq.piece) || sq.col == 1 && next.piece == Piece.EMPTY && IsBlack(sq.piece))
            {
                Square nextBis = nextPos(next, depl);
                if (inBoard(nextBis) && nextBis.piece == Piece.EMPTY) p.Add(nextBis);
            }
            // Attaque
            foreach(Direction d in att)
            {
                
                Square nextAtt = nextPos(sq, d);
                if (inBoard(nextAtt) && nextAtt.piece != Piece.EMPTY && DifferentColor(nextAtt.piece, sq.piece)) p.Add(nextAtt);
            }
            return p;
        }

        private bool inBoard(Square next)
        {
            return next != null && next.col >= 0 && next.col <= 7 && next.row >= 0 && next.row <= 7;
        }

        static HashSet<Direction> Cardinal()
        {
            HashSet<Direction> card = new HashSet<Direction>();
            card.Add(Direction.EST); card.Add(Direction.NORTH); card.Add(Direction.WEST); card.Add(Direction.SOUTH);
            return card;
        }

        static HashSet<Direction> Diagonal()
        {
            HashSet<Direction> diag = new HashSet<Direction>();
            diag.Add(Direction.NORTH_EST); diag.Add(Direction.NORTH_WEST); diag.Add(Direction.SOUTH_WEST); diag.Add(Direction.SOUTH_EST);
            return diag;
        }

        static HashSet<Direction> AllDirection()
        {
            HashSet<Direction> all = new HashSet<Direction>();
            all.Add(Direction.EST); all.Add(Direction.NORTH); all.Add(Direction.WEST); all.Add(Direction.SOUTH);
            all.Add(Direction.NORTH_EST); all.Add(Direction.NORTH_WEST); all.Add(Direction.SOUTH_WEST); all.Add(Direction.SOUTH_EST);
            return all;
        }

        static HashSet<Direction> DiagonalDirection(Direction d)
        {
            HashSet<Direction> dir = new HashSet<Direction>();
            switch (d)
            {
                case Direction.NORTH:
                    dir.Add(Direction.NORTH_WEST); dir.Add(Direction.NORTH_EST); break;
                case Direction.SOUTH:
                    dir.Add(Direction.SOUTH_EST); dir.Add(Direction.SOUTH_WEST); break;
                case Direction.WEST:
                    dir.Add(Direction.NORTH_WEST); dir.Add(Direction.SOUTH_WEST); break;
                case Direction.EST:
                    dir.Add(Direction.NORTH_EST); dir.Add(Direction.SOUTH_EST); break;
            }
            return dir;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            bool blackKingCheck = false; bool whiteKingCheck = false;
            Brush check = new SolidBrush(Color.FromArgb(60, 255, 0, 0));
            if (this.check && blackTurn && !BlackKing.Equals(selection)) { e.Graphics.FillRectangle(check, BlackKing.rectangle); blackKingCheck = true; }
            if (this.check && !blackTurn && !WhiteKing.Equals(selection)) { e.Graphics.FillRectangle(check, WhiteKing.rectangle); whiteKingCheck = true; }
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Square actual = squares[i, j];
                    bool playPos = false;
                    if (selection != null) playPos = PlayablePosSelect.Contains(actual);
                    actual.draw(e.Graphics, actual == selection || playPos || actual.Equals(BlackKing) && blackKingCheck || actual.Equals(WhiteKing) && whiteKingCheck);
                }
            }
            if (selection != null) { 
                Pen p = new Pen(Color.FromArgb(100, 50, 50, 50));
                Brush b = new SolidBrush(Color.FromArgb(60, 255, 255, 0));
                e.Graphics.DrawRectangle(p, selection.rectangle);
                e.Graphics.FillRectangle(b, selection.rectangle);
                
                foreach (Square sq in PlayablePosSelect)
                {
                    e.Graphics.DrawRectangle(p, sq.rectangle);
                    if (sq.piece == Piece.EMPTY) b = new SolidBrush(Color.FromArgb(60, 0, 255, 0));
                    else b = new SolidBrush(Color.FromArgb(60, 255, 0, 0));
                    e.Graphics.FillRectangle(b, sq.rectangle);
                }
            }
        }

        private void Board_MouseDown(object sender, MouseEventArgs e)
        {
            if(PlayablePosSelect.Count != 0)
            {
                Square play = getSquareByClick(e.Location);
                if (selection.Equals(play))
                {
                    selection = null;
                    PlayablePosSelect.Clear();
                    Refresh();
                    return;
                }

                else if (PlayablePosSelect.Contains(play))
                {
                    displacePawn(play);
                    check = false;
                    if (isCheck(!blackTurn))
                    {
                        check = true;
                        MessageBox.Show("Echec !");
                    }
                    
                    nbTurn++;
                    ChangePlayer();
                    PlayablePosSelect.Clear();
                    selection = null;
                    Chargement.Text = "Chargement...";
                    Refresh();
                    
                    actualizePlayablePos();
                    switch (isCheckMateOrDraw(!blackTurn))
                    {
                        case 1:
                            MessageBox.Show("Echec et Mat !");
                            break;
                        case 2:
                            MessageBox.Show("Pat !");
                            break;
                        default: break;
                    }
                    Turn.Text = nbTurn.ToString();
                    Refresh();
                    Chargement.Text = "";
                    return;
                }
            }
            Square previousSelection = selection;
            selection = getSquareByClick(e.Location);
            if (selection == null) { previousSelection = null;  Refresh(); return; }
            PlayablePosSelect.Clear();
            if (IsBlack(selection.piece) == blackTurn)
            {
                HashSet<Square> positions;
                PlayablePos.TryGetValue(selection, out positions);
                PlayablePosSelect.Clear();
                foreach (Square s in positions) PlayablePosSelect.Add(s);
            }
            if (selection.Equals(previousSelection))
            {
                selection = null;
                PlayablePosSelect.Clear();
            }
            Refresh();
        }

        private void actualizePlayablePos()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    selection = squares[i, j];
                    HashSet<Square> play;
                    PlayablePos.TryGetValue(selection, out play);
                    
                    if (selection.piece == Piece.EMPTY || IsBlack(selection.piece) != blackTurn) play.Clear();
                    else
                    {
                        play = playablePos(selection);
                        
                        HashSet<Square> rm = new HashSet<Square>();
                        foreach (Square s in play)
                        {
                            if (MoveCauseCheck(s)) rm.Add(s);
                        }
                        foreach (Square s in rm) play.Remove(s);
                    }
                    PlayablePos[selection] = play;
                }
                
            }
            selection = null;
        }



        private Square getSquareByClick(Point p)
        {
            List<Square> squaresList = squares.Cast<Square>().ToList();
            return squaresList.FirstOrDefault(s => s.rectangle.Contains(p));
        }

        private Square GetSquare(Square sq, int row, int col)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (squares[i, j].Equals(sq))
                    {
                        try
                        {
                            Square next = squares[i + row, j + col];
                            return next;
                        } catch (IndexOutOfRangeException ex)
                        {
                            return null;
                        }
                    }
                }
            }
            return null;
        }
       
    }
}
