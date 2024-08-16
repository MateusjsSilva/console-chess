using ConsoleChess.Entities.Board;
using ConsoleChess.Entities.Board.Enums;
using ConsoleChess.Entities.Board.Exceptions;

namespace ConsoleChess.Entities.Chess
{
    internal class ChessGame
    {
        public Board.Board Board { get; private set; }
        public Color CurrentPlayer { get; private set; }
        public int Turn { get; private set; }
        public bool IsFinished { get; private set; }
        public bool InCheck { get; private set; }
        public Piece VulnerableEnPassant { get; private set; }

        private HashSet<Piece> _pieces;
        private HashSet<Piece> _captured;

        public ChessGame()
        {
            this.Board = new Board.Board(8, 8);
            this._pieces = new HashSet<Piece>();
            this._captured = new HashSet<Piece>();

            this.VulnerableEnPassant = null;
            this.Turn = 1;
            this.CurrentPlayer = Color.White;
            this.IsFinished = false;

            PlacePieces();
        }

        public void AddNewPiece(char column, int row, Piece piece)
        {
            Board.AddPiece(piece, new ChessPosition(column, row).ToPosition());
            _pieces.Add(piece);
        }

        public static Color Opponent(Color color)
        {
            if (color == Color.White)
                return Color.Black;

            return Color.White;
        }

        public bool IsInCheck(Color color)
        {
            Piece king = King(color);
            foreach (Piece piece in GetInPlay(Opponent(color)))
            {
                bool[,] matrix = piece.PossibleMoves();
                if (matrix[king.Position.Row, king.Position.Column])
                    return true;
            }
            return false;
        }

        public Piece ExecuteMove(Position origin, Position destination)
        {
            Piece? piece = Board.RemovePiece(origin);
            Piece? capturedPiece = Board.RemovePiece(destination);

            piece.IncrementMoveCount();
            Board.AddPiece(piece, destination);

            if (capturedPiece != null)
                _captured.Add(capturedPiece);

            // # special move castling kingside
            if (piece is King && destination.Column == origin.Column + 2)
            {
                Piece T = Board.RemovePiece(new Position(origin.Row, origin.Column + 3));
                Board.AddPiece(T, new Position(origin.Row, origin.Column + 1));
                T.IncrementMoveCount();
            }

            // # special move castling queenside
            if (piece is King && destination.Column == origin.Column - 2)
            {
                Piece T = Board.RemovePiece(new Position(origin.Row, origin.Column - 4));
                Board.AddPiece(T, new Position(origin.Row, origin.Column - 1));
                T.IncrementMoveCount();
            }

            // # special move en passant
            if (piece is Pawn)
            {
                if (origin.Column != destination.Column && capturedPiece == null)
                {
                    Position posP;
                    if (piece.Color == Color.White)
                        posP = new Position(destination.Row + 1, destination.Column);
                    else
                        posP = new Position(destination.Row - 1, destination.Column);

                    capturedPiece = Board.RemovePiece(posP);
                    _captured.Add(capturedPiece);
                }
            }

            return capturedPiece;
        }

        private void UndoMove(Position origin, Position destination, Piece capturedPiece)
        {
            Piece piece = Board.RemovePiece(destination);
            piece.DecrementMoveCount();

            if (capturedPiece != null)
            {
                Board.AddPiece(capturedPiece, destination);
                _captured.Remove(capturedPiece);
            }
            Board.AddPiece(piece, origin);

            // # special move castling kingside
            if (piece is King && destination.Column == origin.Column + 2)
            {
                Position originT = new Position(origin.Row, origin.Column + 3);
                Position destinationT = new Position(origin.Row, origin.Column + 1);
                Piece T = Board.RemovePiece(destinationT);
                T.DecrementMoveCount();
                Board.AddPiece(T, originT);
            }

            // # special move castling queenside
            if (piece is King && destination.Column == origin.Column - 2)
            {
                Position originT = new Position(origin.Row, origin.Column - 4);
                Position destinationT = new Position(origin.Row, origin.Column - 1);
                Piece T = Board.RemovePiece(destinationT);
                T.DecrementMoveCount();
                Board.AddPiece(T, originT);
            }

            // # special move en passant
            if (piece is Pawn)
            {
                if (origin.Column != destination.Column && capturedPiece == VulnerableEnPassant)
                {
                    Piece pawn = Board.RemovePiece(destination);

                    Position posP;
                    if (piece.Color == Color.White)
                        posP = new Position(3, destination.Column);
                    else
                        posP = new Position(4, destination.Column);

                    Board.AddPiece(pawn, posP);
                }
            }
        }

        public void MakeMove(Position origin, Position destination)
        {
            Piece capturedPiece = ExecuteMove(origin, destination);

            if (IsInCheck(CurrentPlayer))
            {
                UndoMove(origin, destination, capturedPiece);
                throw new BoardException("You cannot put yourself in check!");
            }

            Piece p = Board.GetPiece(destination);

            // # special move promotion
            if (p is Pawn)
            {
                if ((p.Color == Color.White && destination.Row == 0) || (p.Color == Color.Black && destination.Row == 7))
                {
                    p = Board.RemovePiece(destination);
                    _pieces.Remove(p);
                    Piece queen = new Queen(Board, p.Color);
                    Board.AddPiece(queen, destination);
                    _pieces.Add(queen);
                }
            }

            if (IsInCheck(Opponent(CurrentPlayer)))
                InCheck = true;
            else
                InCheck = false;

            if (IsCheckmate(Opponent(CurrentPlayer)))
            {
                IsFinished = true;
            }
            else
            {
                Turn++;
                ChangePlayer();
            }

            p = Board.GetPiece(destination);

            // # special move en passant
            if (p is Pawn && (destination.Row == origin.Row - 2 || destination.Row == origin.Row + 2))
                VulnerableEnPassant = p;
            else
                VulnerableEnPassant = null;
        }

        public bool IsCheckmate(Color color)
        {
            if (!IsInCheck(color))
                return false;

            foreach (Piece piece in GetInPlay(color))
            {
                bool[,] moves = piece.PossibleMoves();
                for (int i = 0; i < Board.Rows; i++)
                {
                    for (int j = 0; j < Board.Columns; j++)
                    {
                        if (moves[i, j])
                        {
                            Position origin = piece.Position;
                            Position destination = new Position(i, j);
                            Piece capturedPiece = ExecuteMove(origin, destination);
                            bool checkTest = IsInCheck(color);
                            UndoMove(origin, destination, capturedPiece);
                            if (!checkTest)
                                return false;
                        }
                    }
                }
            }
            return true;
        }

        public void ValidateOriginPosition(Position position)
        {
            if (Board.GetPiece(position) == null)
                throw new BoardException("There is no piece in the selected position!");

            if (CurrentPlayer != Board.GetPiece(position).Color)
                throw new BoardException("The selected piece is not yours!");

            if (!Board.GetPiece(position).HasPossibleMoves())
                throw new BoardException("The selected piece cannot be moved!");
        }

        public void ValidateDestinationPosition(Position origin, Position destination)
        {
            if (!Board.GetPiece(origin).IsMovePossible(destination))
                throw new BoardException("You cannot move to that position!");
        }

        private void ChangePlayer()
        {
            CurrentPlayer = CurrentPlayer == Color.White ? Color.Black : Color.White;
        }

        private Piece King(Color color)
        {
            foreach (Piece piece in GetInPlay(color))
                if (piece is King)
                    return piece;

            throw new BoardException("There is no " + color + " King on the board!");
        }

        public HashSet<Piece> GetCaptured(Color color)
        {
            HashSet<Piece> aux = new HashSet<Piece>();
            foreach (Piece x in _captured)
            {
                if (x.Color == color)
                    aux.Add(x);
            }
            return aux;
        }

        public HashSet<Piece> GetInPlay(Color color)
        {
            HashSet<Piece> aux = new HashSet<Piece>();
            foreach (Piece x in _pieces)
            {
                if (x.Color == color)
                    aux.Add(x);
            }
            aux.ExceptWith(GetCaptured(color));
            return aux;
        }

        private void PlacePieces()
        {
            // White pieces
            AddNewPiece('a', 1, new Rook(Board, Color.White));
            AddNewPiece('b', 1, new Knight(Board, Color.White));
            AddNewPiece('c', 1, new Bishop(Board, Color.White));
            AddNewPiece('d', 1, new Queen(Board, Color.White));
            AddNewPiece('e', 1, new King(Board, Color.White, this));
            AddNewPiece('f', 1, new Bishop(Board, Color.White));
            AddNewPiece('g', 1, new Knight(Board, Color.White));
            AddNewPiece('h', 1, new Rook(Board, Color.White));

            AddNewPiece('a', 2, new Pawn(Board, Color.White, this));
            AddNewPiece('b', 2, new Pawn(Board, Color.White, this));
            AddNewPiece('c', 2, new Pawn(Board, Color.White, this));
            AddNewPiece('d', 2, new Pawn(Board, Color.White, this));
            AddNewPiece('e', 2, new Pawn(Board, Color.White, this));
            AddNewPiece('f', 2, new Pawn(Board, Color.White, this));
            AddNewPiece('g', 2, new Pawn(Board, Color.White, this));
            AddNewPiece('h', 2, new Pawn(Board, Color.White, this));

            // Black pieces
            AddNewPiece('a', 8, new Rook(Board, Color.Black));
            AddNewPiece('b', 8, new Knight(Board, Color.Black));
            AddNewPiece('c', 8, new Bishop(Board, Color.Black));
            AddNewPiece('d', 8, new Queen(Board, Color.Black));
            AddNewPiece('e', 8, new King(Board, Color.Black, this));
            AddNewPiece('f', 8, new Bishop(Board, Color.Black));
            AddNewPiece('g', 8, new Knight(Board, Color.Black));
            AddNewPiece('h', 8, new Rook(Board, Color.Black));

            AddNewPiece('a', 7, new Pawn(Board, Color.Black, this));
            AddNewPiece('b', 7, new Pawn(Board, Color.Black, this));
            AddNewPiece('c', 7, new Pawn(Board, Color.Black, this));
            AddNewPiece('d', 7, new Pawn(Board, Color.Black, this));
            AddNewPiece('e', 7, new Pawn(Board, Color.Black, this));
            AddNewPiece('f', 7, new Pawn(Board, Color.Black, this));
            AddNewPiece('g', 7, new Pawn(Board, Color.Black, this));
            AddNewPiece('h', 7, new Pawn(Board, Color.Black, this));
        }
    }
}