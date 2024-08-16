using ConsoleChess.Entities.Board;
using ConsoleChess.Entities.Board.Enums;

namespace ConsoleChess.Entities.Chess
{
    internal class King : Piece
    {
        private ChessGame _game;

        public King(Board.Board board, Color color, ChessGame game) : base(board, color)
        {
            this._game = game;
        }

        public override bool[,] PossibleMoves()
        {
            bool[,] matrix = new bool[Board.Rows, Board.Columns];

            Position pos = new Position(0, 0);

            // above
            pos.SetValues(Position.Row - 1, Position.Column);
            if (Board.IsValidPosition(pos) && CanMove(pos))
                matrix[pos.Row, pos.Column] = true;

            // northeast
            pos.SetValues(Position.Row - 1, Position.Column + 1);
            if (Board.IsValidPosition(pos) && CanMove(pos))
                matrix[pos.Row, pos.Column] = true;

            // right
            pos.SetValues(Position.Row, Position.Column + 1);
            if (Board.IsValidPosition(pos) && CanMove(pos))
                matrix[pos.Row, pos.Column] = true;

            // southeast
            pos.SetValues(Position.Row + 1, Position.Column + 1);
            if (Board.IsValidPosition(pos) && CanMove(pos))
                matrix[pos.Row, pos.Column] = true;

            // below
            pos.SetValues(Position.Row + 1, Position.Column);
            if (Board.IsValidPosition(pos) && CanMove(pos))
                matrix[pos.Row, pos.Column] = true;

            // southwest
            pos.SetValues(Position.Row + 1, Position.Column - 1);
            if (Board.IsValidPosition(pos) && CanMove(pos))
                matrix[pos.Row, pos.Column] = true;

            // left
            pos.SetValues(Position.Row, Position.Column - 1);
            if (Board.IsValidPosition(pos) && CanMove(pos))
                matrix[pos.Row, pos.Column] = true;

            // northwest
            pos.SetValues(Position.Row - 1, Position.Column - 1);
            if (Board.IsValidPosition(pos) && CanMove(pos))
                matrix[pos.Row, pos.Column] = true;

            // # special move castling
            if (MoveCount == 0 && !_game.InCheck)
            {
                // # special move kingside castling
                Position posR1 = new Position(Position.Row, Position.Column + 3);
                if (TestRookForCastling(posR1))
                {
                    Position p1 = new Position(Position.Row, Position.Column + 1);
                    Position p2 = new Position(Position.Row, Position.Column + 2);
                    if (Board.GetPiece(p1) == null && Board.GetPiece(p2) == null)
                    {
                        matrix[Position.Row, Position.Column + 2] = true;
                    }
                }

                // # special move queenside castling
                Position posR2 = new Position(Position.Row, Position.Column - 4);
                if (TestRookForCastling(posR2))
                {
                    Position p1 = new Position(Position.Row, Position.Column - 1);
                    Position p2 = new Position(Position.Row, Position.Column - 2);
                    Position p3 = new Position(Position.Row, Position.Column - 3);
                    if (Board.GetPiece(p1) == null && Board.GetPiece(p2) == null && Board.GetPiece(p3) == null)
                    {
                        matrix[Position.Row, Position.Column - 2] = true;
                    }
                }
            }
            return matrix;
        }

        public override bool CanMove(Position position)
        {
            Piece p = Board.GetPiece(position);
            return p == null || p.Color != this.Color;
        }

        public bool TestRookForCastling(Position position)
        {
            Piece p = Board.GetPiece(position);
            return p != null && p is Rook && p.Color == Color && p.MoveCount == 0;
        }

        public override string ToString()
        {
            return "K";
        }
    }
}