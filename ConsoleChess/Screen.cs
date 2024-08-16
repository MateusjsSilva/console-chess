using ConsoleChess.Entities.Board;
using ConsoleChess.Entities.Board.Enums;
using ConsoleChess.Entities.Chess;

namespace ConsoleChess
{
    internal class Screen
    {
        public static void PrintGame(ChessGame chessGame, bool[,] possiblePositions)
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Gray;

            Console.WriteLine("----------------------------------------");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("                CHESS             ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("----------------------------------------\n");

            PrintBoard(chessGame.Board, possiblePositions);
            PrintCapturedPieces(chessGame);

            if (!chessGame.IsFinished)
            {
                Console.WriteLine($" Turn: {chessGame.Turn}     Waiting: {chessGame.CurrentPlayer}");
                Console.ForegroundColor = ConsoleColor.Red;
                if (chessGame.InCheck) Console.WriteLine("You are in CHECK!");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($" Turn: {chessGame.Turn}    CHECKMATE!  Winner: {chessGame.CurrentPlayer}");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            Console.WriteLine("----------------------------------------");
        }

        public static void PrintBoard(Board board, bool[,] possiblePositions)
        {
            ConsoleColor originalBackground = ConsoleColor.Black;
            ConsoleColor background1 = ConsoleColor.Blue;
            ConsoleColor background2 = ConsoleColor.DarkBlue;

            byte[,] background =
            {
                {1,0,1,0,1,0,1,0},
                {0,1,0,1,0,1,0,1},
                {1,0,1,0,1,0,1,0},
                {0,1,0,1,0,1,0,1},
                {1,0,1,0,1,0,1,0},
                {0,1,0,1,0,1,0,1},
                {1,0,1,0,1,0,1,0},
                {0,1,0,1,0,1,0,1}
            };

            if (possiblePositions != null)
            {
                for (int i = 0; i < board.Rows; i++)
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write("       " + (8 - i) + " ");

                    for (int j = 0; j < board.Columns; j++)
                    {
                        if (possiblePositions[i, j])
                        {
                            if (board.PieceExists(new Position(i, j)))
                                Console.BackgroundColor = ConsoleColor.DarkRed;
                            else
                                Console.BackgroundColor = ConsoleColor.DarkYellow;
                        }
                        else
                        {
                            if (background[i, j] == 1)
                                Console.BackgroundColor = background1;
                            else
                                Console.BackgroundColor = background2;
                        }
                        PrintPiece(board.GetPiece(i, j));
                        Console.BackgroundColor = originalBackground;
                    }
                    Console.WriteLine();
                }
            }
            else
            {
                for (int i = 0; i < board.Rows; i++)
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write("       " + (8 - i) + " ");

                    for (int j = 0; j < board.Columns; j++)
                    {
                        if (background[i, j] == 1)
                            Console.BackgroundColor = background1;
                        else
                            Console.BackgroundColor = background2;
                        PrintPiece(board.GetPiece(i, j));
                    }
                    Console.WriteLine();
                    Console.BackgroundColor = originalBackground;
                }
            }

            Console.BackgroundColor = originalBackground;
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("          a  b  c  d  e  f  g  h\n");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private static void PrintCapturedPieces(ChessGame game)
        {
            Console.WriteLine("----------------------------------------");
            Console.Write(" White: ");

            Console.ForegroundColor = ConsoleColor.Yellow;
            PrintSet(game.GetCaptured(Color.White));
            Console.ForegroundColor = ConsoleColor.Gray;

            Console.Write(" Black: ");

            Console.ForegroundColor = ConsoleColor.Yellow;
            PrintSet(game.GetCaptured(Color.Black));
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("----------------------------------------");
        }

        private static void PrintSet(HashSet<Piece> pieces)
        {
            bool display = false;
            Console.Write("[");
            foreach (Piece piece in pieces)
            {
                if (display)
                {
                    Console.Write("," + piece);
                }
                else
                {
                    Console.Write(piece);
                    display = true;
                }
            }
            Console.WriteLine("]");
        }

        public static ChessPosition ReadChessPosition()
        {
            string input = Console.ReadLine();
            char column = input[0];
            int row = int.Parse(input[1] + "");
            return new ChessPosition(column, row);
        }

        public static void PrintPiece(Piece piece)
        {
            if (piece == null)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("   ");
            }
            else
            {
                if (piece.Color == Color.Black)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write(" " + piece + " ");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(" " + piece + " ");
                }
            }
        }
    }
}