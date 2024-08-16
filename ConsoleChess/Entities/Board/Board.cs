using ConsoleChess.Entities.Board.Exceptions;

namespace ConsoleChess.Entities.Board
{
    internal class Board
    {
        public int Rows { get; set; }
        public int Columns { get; set; }

        private Piece[,] _pieces;

        public Board(int rows, int columns)
        {
            this.Rows = rows;
            this.Columns = columns;
            this._pieces = new Piece[rows, columns];
        }

        public void AddPiece(Piece piece, Position position)
        {
            if (PieceExists(position))
                throw new BoardException("A piece already exists at this position!");

            _pieces[position.Row, position.Column] = piece;
            piece.Position = position;
        }

        public Piece? RemovePiece(Position position)
        {
            if (!PieceExists(position))
                return null;

            Piece tempPiece = _pieces[position.Row, position.Column];
            _pieces[position.Row, position.Column] = null;
            return tempPiece;
        }

        public bool PieceExists(Position position)
        {
            ValidatePosition(position);
            return GetPiece(position) != null;
        }

        public bool IsValidPosition(Position position)
        {
            if (position.Row < 0 || position.Row >= Rows || position.Column < 0 || position.Column >= Columns)
                return false;
            return true;
        }

        public void ValidatePosition(Position position)
        {
            if (!IsValidPosition(position))
                throw new BoardException("Invalid position!");
        }

        public Piece GetPiece(int row, int column)
        {
            return _pieces[row, column];
        }

        public Piece GetPiece(Position position)
        {
            return _pieces[position.Row, position.Column];
        }
    }
}