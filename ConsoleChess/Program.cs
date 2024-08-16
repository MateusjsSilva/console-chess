using ConsoleChess.Entities.Board.Exceptions;
using ConsoleChess.Entities.Board;
using ConsoleChess.Entities.Chess;

namespace ConsoleChess
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ChessGame chessGame = new ChessGame();

            while (!chessGame.IsFinished)
            {
                try
                {
                    Screen.PrintGame(chessGame, null);

                    Console.Write(" Origin: ");
                    Position origin = Screen.ReadChessPosition().ToPosition();
                    chessGame.ValidateOriginPosition(origin);

                    Screen.PrintGame(chessGame, chessGame.Board.GetPiece(origin).PossibleMoves());

                    Console.Write(" Destination: ");
                    Position destination = Screen.ReadChessPosition().ToPosition();
                    chessGame.ValidateDestinationPosition(origin, destination);

                    chessGame.MakeMove(origin, destination);
                }
                catch (BoardException e)
                {
                    Console.WriteLine($"Error: {e.Message} Press [ENTER] to continue!");
                    Console.ReadLine();
                }
            }

            Screen.PrintGame(chessGame, null);
        }
    }
}