using System.Runtime.InteropServices;
using System.Text;
using static ChessBoard.ProgramHelpers;

namespace ChessBoard
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Chessboard Writer");
            bool onWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            if (onWindows)
            {
                Console.OutputEncoding = System.Text.Encoding.Unicode;
                Console.InputEncoding = System.Text.Encoding.Unicode;
                Console.WriteLine("Running on windows. To use emojis, download Windows Terminal\n" +
                    "https://www.microsoft.com/store/productId/9N0DX20HK701\n" +
                    "if you can't see a crown here: 👑");
                Console.WriteLine("Otherwise use alphanumerical characters and simpler symbols");
            }
            else
            {
                Console.OutputEncoding = System.Text.Encoding.Default;
                Console.InputEncoding = System.Text.Encoding.Default;
                Console.WriteLine("You're on a mac, great! Hope you're using iTerm!");
            }

            const ushort MaxSize = 24;

            string sizeString = ReadAndValidateInput(
                message: $"Enter size of chessboard (1-{MaxSize}). Default is 8: ",
                errorMessage: $"Enter a _number_ between 1 and {MaxSize}: ",
                inputValidator: NumberInRangeValidator,
                constraints: new { start = 1, end = MaxSize }); 

            ushort size = ushort.Parse(sizeString);
            Console.WriteLine($"Chessboard Size: {size}x{size}");

            if (onWindows)
            {
                Console.WriteLine(String.Format("If pasting or inserting emoji character, input will look like {0}{0} or {0}.", Rune.ReplacementChar));
                Console.WriteLine("Don't worry, Output will be preserved");
            }
            string blackSquare = ReadSquare(symbolName: "black squares", defaultSymbol: "■");
            string whiteSquare = ReadSquare(symbolName: "white squares", defaultSymbol: "□");
            string piece = ReadSquare(symbolName: "chess piece", defaultSymbol: "♛");

            /* 
             * Squares for the chessboard grid stored as array so we can use the
             * 0-1 index when alternating between 0 and 1 in pattern generation.
             */
            string[] gridSquares = { whiteSquare, blackSquare, piece };
            bool wideChars = false;
            // If at least one symbol is wide the entire chessboard needs to be.
            int symbolIdx = 0;
            while (symbolIdx < gridSquares.Length && !wideChars)
            {
                wideChars = SymbolIsWide(gridSquares[symbolIdx++]);
            }

            (ushort row, ushort col) = ReadCoordinatesFromNotation(size);

            // Generate chessboard as 2-dimensional array.
            string[,] chessBoard = new string[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    chessBoard[i, j] = gridSquares[(i + j) % 2];
                }
            }
            // Place piece char in chessboard array.
            chessBoard[size - row - 1, col] = piece;

            // Write column headers.
            Console.Write("   ");
            for (char columnLetter = 'A'; columnLetter < (char)('A' + size); columnLetter++)
            {
                Console.Write(columnLetter);
                // Some symbols need extra spacing so this aligns the column headers.
                if (wideChars)
                {
                    Console.Write(" ");
                }
            }
            Console.WriteLine();

            // Write chessboard array
            for (int i = 0; i < size; i++)
            {
                // This is the rowheader. Width is dependent on size of board.
                Console.Write($"{size - i,-3}");
                for (int j = 0; j < size; j++)
                {
                    var currentSquare = chessBoard[i, j];
                    Console.Write(currentSquare);
                    if (wideChars)
                    {
                        // Some symbols need extra spacing to align with wider symbols.
                        if (currentSquare.Length == 1 || !SymbolIsWide(currentSquare))
                        {
                            Console.Write(" ");
                        }
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}