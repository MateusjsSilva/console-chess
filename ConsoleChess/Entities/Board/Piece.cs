using ConsoleChess.Entities.Board.Enums;

namespace ConsoleChess.Entities.Board
{
    abstract internal class Piece 
    {
        public Position? Position { get; set; }
        public Color Color { get; protected set; }
        public int MoveCount { get; protected set; }
        public Board Board { get; set; }

        public Piece(Board board, Color color)
        {
            this.Position = null;
            this.Color = color;
            this.Board = board;
            this.MoveCount = 0;
        }

        public bool HasPossibleMoves()
        {
            bool[,] matrix = PossibleMoves();
            for (int i = 0; i < Board.Rows; i++)
            {
                for (int j = 0; j < Board.Columns; j++)
                {
                    if (matrix[i, j])
                        return true;
                }
            }
            return false;
        }

        public bool IsMovePossible(Position position)
        {
            return PossibleMoves()[position.Row, position.Column];
        }

        public void IncrementMoveCount()
        {
            this.MoveCount++;
        }

        public void DecrementMoveCount()
        {
            this.MoveCount--;
        }

        public abstract bool[,] PossibleMoves();

        public abstract bool CanMove(Position position);
    }
}