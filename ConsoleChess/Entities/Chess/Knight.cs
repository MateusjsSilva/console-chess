using ConsoleChess.Entities.Board;
using ConsoleChess.Entities.Board.Enums;

namespace ConsoleChess.Entities.Chess
{
    internal class Knight : Piece
    {
        private bool[,] _matrix;
        private Position _position;

        public Knight(Board.Board board, Color color) : base(board, color) { }

        public override bool[,] PossibleMoves()
        {
            _matrix = new bool[Board.Rows, Board.Columns];

            _position = new Position(0, 0);

            CheckPosition(-1, -2);
            CheckPosition(-2, -1);
            CheckPosition(-2, +1);
            CheckPosition(-1, +2);
            CheckPosition(+1, +2);
            CheckPosition(+2, +1);
            CheckPosition(+2, -1);
            CheckPosition(+1, -2);

            return _matrix;
        }

        private void CheckPosition(int rowModifier, int columnModifier)
        {
            _position.SetValues(Position.Row + rowModifier, Position.Column + columnModifier);
            if (Board.IsValidPosition(_position) && CanMove(_position))
                _matrix[_position.Row, _position.Column] = true;
        }

        public override bool CanMove(Position position)
        {
            Piece p = Board.GetPiece(position);
            return p == null || p.Color != Color;
        }

        public override string ToString()
        {
            return "N";
        }
    }
}