using System;
using System.Globalization;
using System.Text;

namespace ChessBoard
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Default;
            const ushort MaxSize = 24;

            // Interface with the user
            Console.Write($"Enter size of chessboard (1-{MaxSize}): ");
            ushort size;
            while (true)
            {
                if (ushort.TryParse(Console.ReadLine(), out size))
                {
                    if (InRange(size, 1, MaxSize))
                    {
                        break;
                    }
                }
                Console.Write($"Enter a _number_ between 1 and {MaxSize}: ");
            }
            Console.WriteLine($"Chessboard Size: {size}x{size}");

            Console.WriteLine("Enter colors of squares, leave blank to keep default.");
            string blackSquare = getSquare("black squares", "■");
            Console.WriteLine($"black: {blackSquare}, length: {blackSquare.Length}");
            string whiteSquare = getSquare("white squares", "□");
            Console.WriteLine($"white: {whiteSquare}, length: {whiteSquare.Length}");
            string piece = getSquare("chess piece", "♛");
            Console.WriteLine($"piece: {piece}, length: {piece.Length}");

            /* 
             * Squares for the chessboard grid stored as array so we can use the
             * 0-1 index when alternating between 0 and 1 in pattern generation.
             */
            string[] gridSquares = { whiteSquare, blackSquare, piece };
            bool wideChars = false;
            foreach (string s in gridSquares)
            {
                if (s.Length > 1)
                {
                    wideChars = true;
                }
            }
            Console.WriteLine($"Wide characters: {wideChars}");

            (ushort row, ushort col) = GetCoordinatesFromNotation(size);

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
            chessBoard[size - 1 - row, col] = piece;

            // Write column headers.
            Console.Write("   ");
            for (char letter = 'A'; letter < (char)('A' + size); letter++)
            {
                Console.Write(letter);
                if (wideChars)
                {
                    Console.Write(" ");
                }
            }
            Console.WriteLine();
            // Write chessboard array
            for (int i = 0; i < size; i++)
            {
                Console.Write($"{size - i,-3}");
                for (int j = 0; j < size; j++)
                {
                    var currentSquare = chessBoard[i, j];
                    Console.Write(currentSquare);
                    if (wideChars)
                    {
                        if (currentSquare.Length == 1 || !symbolIsWide(currentSquare))
                        {
                            Console.Write(" ");
                        }
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        static string getSquare(string item, string defaultSymbol)
        {
            bool accepted = false;
            string square = "";
            do
            {
                Console.Write($"Enter character for {item} (default {defaultSymbol}): ");
                string input = Console.ReadLine() ?? "";
                Console.WriteLine("input: " + input);
                // Counts graphemes instead of length because emojis are sequenses of UTF16 characters.
                var si = new StringInfo(input);
                Console.WriteLine("stringinfo length elements: " + si.LengthInTextElements);
                if (si.LengthInTextElements <= 1)
                {
                    square = si.LengthInTextElements == 0 ? defaultSymbol : input;
                    Console.WriteLine($"square: {square}");
                    accepted = true;
                }
                else
                {
                    Console.WriteLine("Invalid entry, enter a _single_ character or leave blank.");
                }
            } while (!accepted);
            return square;
        }

        static (ushort row, ushort col) GetCoordinatesFromNotation(ushort size)
        {
            bool accepted = false;
            ushort row = 0;
            ushort col = 0;

            do
            {
                Console.Write($"Enter position: (A-{(char)('A' + size - 1)})(1-{size}): ");
                var input = Console.ReadLine() ?? "";
                if (InRange(input.Length, 2, 3))
                {
                    char columnChar = input[0];
                    if (InRange(columnChar, 'A', 'A' + size))
                    {
                        col = (ushort)(columnChar - 'A');
                        string rowChars = input[1..^0];
                        if (ushort.TryParse(rowChars, out row))
                        {
                            row--;
                            accepted = InRange(row, 0, size - 1);
                        }
                    }
                }
                if (!accepted)
                {
                    Console.WriteLine("Invalid entry or position out of range.");
                }
            } while (!accepted);
            return (row, col);
        }

        static bool InRange(int value, int start, int end)
        {
            return value >= start && value <= end;
        }

        static bool symbolIsWide(string s)
        {
            return (s.EnumerateRunes().Count() >= 1);
        }
    }
}