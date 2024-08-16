using ConsoleChess.Entities.Board;
using ConsoleChess.Entities.Board.Enums;

namespace ConsoleChess.Entities.Chess
{
    internal class Pawn : Piece
    {
        private ChessGame _game;

        public Pawn(Board.Board board, Color color, ChessGame game) : base(board, color)
        {
            this._game = game;
        }

        public override bool[,] PossibleMoves()
        {
            bool[,] matrix = new bool[Board.Rows, Board.Columns];

            Position pos = new Position(0, 0);

            if (Color == Color.White)
            {
                pos.SetValues(Position.Row - 1, Position.Column);
                if (Board.IsValidPosition(pos) && IsFree(pos))
                    matrix[pos.Row, pos.Column] = true;

                pos.SetValues(Position.Row - 2, Position.Column);
                if (Board.IsValidPosition(pos) && IsFree(pos) && MoveCount == 0)
                    matrix[pos.Row, pos.Column] = true;

                pos.SetValues(Position.Row - 1, Position.Column - 1);
                if (Board.IsValidPosition(pos) && CanMove(pos))
                    matrix[pos.Row, pos.Column] = true;

                pos.SetValues(Position.Row - 1, Position.Column + 1);
                if (Board.IsValidPosition(pos) && CanMove(pos))
                    matrix[pos.Row, pos.Column] = true;

                // # special move en passant
                if (Position.Row == 3)
                {
                    Position left = new Position(Position.Row, Position.Column - 1);
                    if (Board.IsValidPosition(left) && CanMove(left) && Board.GetPiece(left) == _game.VulnerableEnPassant)
                    {
                        matrix[left.Row - 1, left.Column] = true;
                    }
                    Position right = new Position(Position.Row, Position.Column + 1);
                    if (Board.IsValidPosition(right) && CanMove(right) && Board.GetPiece(right) == _game.VulnerableEnPassant)
                    {
                        matrix[right.Row - 1, right.Column] = true;
                    }
                }
            }
            else
            {
                pos.SetValues(Position.Row + 1, Position.Column);
                if (Board.IsValidPosition(pos) && IsFree(pos))
                    matrix[pos.Row, pos.Column] = true;

                pos.SetValues(Position.Row + 2, Position.Column);
                if (Board.IsValidPosition(pos) && IsFree(pos) && MoveCount == 0)
                    matrix[pos.Row, pos.Column] = true;

                pos.SetValues(Position.Row + 1, Position.Column - 1);
                if (Board.IsValidPosition(pos) && CanMove(pos))
                    matrix[pos.Row, pos.Column] = true;

                pos.SetValues(Position.Row + 1, Position.Column + 1);
                if (Board.IsValidPosition(pos) && CanMove(pos))
                    matrix[pos.Row, pos.Column] = true;

                // # special move en passant
                if (Position.Row == 4)
                {
                    Position left = new Position(Position.Row, Position.Column - 1);
                    if (Board.IsValidPosition(left) && CanMove(left) && Board.GetPiece(left) == _game.VulnerableEnPassant)
                    {
                        matrix[left.Row + 1, left.Column] = true;
                    }
                    Position right = new Position(Position.Row, Position.Column + 1);
                    if (Board.IsValidPosition(right) && CanMove(right) && Board.GetPiece(right) == _game.VulnerableEnPassant)
                    {
                        matrix[right.Row + 1, right.Column] = true;
                    }
                }
            }

            return matrix;
        }

        private bool IsFree(Position position)
        {
            return Board.GetPiece(position) == null;
        }

        public override bool CanMove(Position position)
        {
            Piece p = Board.GetPiece(position);
            return p != null && p.Color != Color;
        }

        public override string ToString()
        {
            return "P";
        }
    }
}