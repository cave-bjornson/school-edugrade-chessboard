using System.Runtime.InteropServices;
using System.Text;
using static ChessBoard.InputValidation;
using static ChessBoard.ProgramHelpers;

/*
 * This writes a chessboard on the console with properties depending on input.
 * The solution is 200% overkill for the assignment but I wanted to refresh my
 * programming after the summer. I also wanted to try out some things that
 * is not available in Java. Tuples is among them.
 * 
 * Björn Agnemo
 * NET22
 */
namespace ChessBoard
{
    internal class Program
    {
        static void Main()
        {
            // Max XY-size of grid. Under the limit of available column letters.
            const int MaxSize = 24;

            Console.WriteLine("Chessboard Writer!");

            // Gives some platform specific advise
            bool onWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            bool onMac = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
            if (onWindows)
            {
                Console.OutputEncoding = Encoding.Unicode;
                Console.InputEncoding = Encoding.Unicode;
                Console.WriteLine("Running on windows. Can you see a crown here: 👑? No? To use emojis,\n" +
                                  "download Windows Terminal https://www.microsoft.com/store/productId/9N0DX20HK701\n" +
                                  "Otherwise use alphanumerical characters and other simple symbols");
            }
            else if (onMac)
            {
                Console.OutputEncoding = Encoding.Default;
                Console.InputEncoding = Encoding.Default;
                Console.WriteLine("You're on a mac, great! Hope you're using iTerm!");
            }
            else
            {
                Console.WriteLine("You're running on linux, you're on your own here!");
            }

            int size = ReadAndValidateInput(
                message: $"Enter size of chessboard (1-{MaxSize}). Default is 8: ",
                errorMessage: $"Enter a _number_ between 1 and {MaxSize}: ",
                inputValidator: (s) => NumberInRangeValidator(s, lower: 1, upper: MaxSize),
                onAccepted: (string s) => int.Parse(s),
                defaultInput: "8");

            Console.WriteLine($"Chessboard Size: {size}x{size}");

            /*
             * Even when using windows terminal, input characters on the readline
             * will not be shown properly.
             */
            if (onWindows)
            {
                Console.WriteLine(String.Format("If pasting or inserting emoji character, " +
                    "input will look like {0}{0} or {0}.", Rune.ReplacementChar));
                Console.WriteLine("Don't worry, Output will be preserved");
            }

            // Local function to handle multiples chess-symbol inputs
            string ReadSymbol(string name, string defaultSymbol) => ReadAndValidateInput(
                message: $"Enter character for {name} (default {defaultSymbol}): ",
                errorMessage: "Invalid entry, enter a _single_ character or leave blank.",
                inputValidator: SymbolValidator,
                defaultInput: defaultSymbol,
                onAccepted: (input) => { Console.WriteLine($"{name} is {input}"); return input; }
                );

            string blackSquare = ReadSymbol(name: "black squares", defaultSymbol: "#");
            string whiteSquare = ReadSymbol(name: "white squares", defaultSymbol: ".");
            string piece = ReadSymbol(name: "chess piece", defaultSymbol: "Q");

            /* 
             * Squares for the chessboard grid stored as array so we can use the
             * 0-1 index when alternating between 0 and 1 in pattern generation.
             */
            string[] gridSquares = { whiteSquare, blackSquare };

            // Gets the XY-coordinates
            (char col, int row) = ReadAndValidateInput(
                message: $"Enter position: (A-{(char)('A' + size - 1)})(1-{size}): ",
                errorMessage: "Invalid entry or position out of range.",
                inputValidator: (s) => ChessCoordValidator(s, size),
                onAccepted: (s) => (char.ToUpper(s[0]), int.Parse(s[1..])),
                defaultInput: "A1"
                );

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
            (int arrayRow, int arrayCol) = ChessXYToIndices(col, row, size);
            chessBoard.SetValue(piece, arrayRow, arrayCol);

            // Write column headers.
            Console.Write("   ");
            for (char columnLetter = 'A'; columnLetter < (char)('A' + size); columnLetter++)
            {
                Console.Write(columnLetter + " ");
            }
            Console.WriteLine();

            // Write chessboard array
            for (int i = 0; i < size; i++)
            {
                // This is the rowheader. 1-2 character width is dependent on size of board.
                Console.Write($"{size - i,-3}");
                for (int j = 0; j < size; j++)
                {
                    var currentSquare = chessBoard[i, j];
                    Console.Write(currentSquare);
                    // Some symbols need extra spacing to align with wider symbols.
                    if (currentSquare.Length == 1 || !SymbolIsWide(currentSquare))
                    {
                        Console.Write(" ");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}