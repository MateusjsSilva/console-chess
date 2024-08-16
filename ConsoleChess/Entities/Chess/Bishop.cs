using ConsoleChess.Entities.Board;
using ConsoleChess.Entities.Board.Enums;

namespace ConsoleChess.Entities.Chess
{
    internal class Bishop : Piece
    {
        private bool[,] _matrix;
        private Position _position;

        public Bishop(Board.Board board, Color color) : base(board, color) { }

        public override bool[,] PossibleMoves()
        {
            _matrix = new bool[Board.Rows, Board.Columns];

            _position = new Position(0, 0);

            CheckPosition(-1, -1); // northwest
            CheckPosition(-1, +1); // northeast
            CheckPosition(+1, -1); // southwest
            CheckPosition(+1, +1); // southeast

            return _matrix;
        }

        private void CheckPosition(int rowModifier, int columnModifier)
        {
            if (Position == null)
                return;

            _position.SetValues(Position.Row + rowModifier, Position.Column + columnModifier);
            while (Board.IsValidPosition(_position) && CanMove(_position))
            {
                _matrix[_position.Row, _position.Column] = true;
                if (Board.GetPiece(_position) != null && Board.GetPiece(_position).Color != Color)
                    break;
                _position.SetValues(_position.Row + rowModifier, _position.Column + columnModifier);
            }
        }

        public override bool CanMove(Position position)
        {
            Piece p = Board.GetPiece(position);
            return p == null || p.Color != Color;
        }

        public override string ToString()
        {
            return "B";
        }
    }
}