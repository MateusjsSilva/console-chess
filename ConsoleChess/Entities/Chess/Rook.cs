using ConsoleChess.Entities.Board;
using ConsoleChess.Entities.Board.Enums;

namespace ConsoleChess.Entities.Chess
{
    internal class Rook : Piece
    {
        private bool[,] _matrix;
        private Position _position;

        public Rook(Board.Board board, Color color) : base(board, color) { }

        public override bool[,] PossibleMoves()
        {
            _matrix = new bool[Board.Rows, Board.Columns];

            _position = new(0, 0);

            // above
            _position.SetValues(Position.Row - 1, Position.Column);
            CheckVerticalMove(-1);

            // below
            _position.SetValues(Position.Row + 1, Position.Column);
            CheckVerticalMove(+1);

            // right
            _position.SetValues(Position.Row, Position.Column + 1);
            CheckHorizontalMove(+1);

            // left
            _position.SetValues(Position.Row, Position.Column - 1);
            CheckHorizontalMove(-1);

            return _matrix;
        }

        private void CheckVerticalMove(int rowModifier)
        {
            while (Board.IsValidPosition(_position) && CanMove(_position))
            {
                _matrix[_position.Row, _position.Column] = true;
                if (Board.GetPiece(_position) != null && Board.GetPiece(_position).Color != this.Color)
                    break;
                _position.Row += rowModifier;
            }
        }

        private void CheckHorizontalMove(int columnModifier)
        {
            while (Board.IsValidPosition(_position) && CanMove(_position))
            {
                _matrix[_position.Row, _position.Column] = true;
                if (Board.GetPiece(_position) != null && Board.GetPiece(_position).Color != this.Color)
                    break;
                _position.Column += columnModifier;
            }
        }

        public override bool CanMove(Position position)
        {
            Piece p = Board.GetPiece(position);
            return p == null || p.Color != Color;
        }

        public override string ToString()
        {
            return "R";
        }
    }
}