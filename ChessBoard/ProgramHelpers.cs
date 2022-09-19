using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ChessBoard
{
    internal static class ProgramHelpers
    {
        public static bool InRange(int value, int start, int end)
        {
            return value >= start && value <= end;
        }

        public static string ReadAndValidateInput(string message, string errorMessage, Func<string, dynamic, bool> inputValidator, dynamic constraints)
        {
            bool accepted;
            string input;
            do
            {
                Console.WriteLine(message);
                input = Console.ReadLine() ?? "";
                accepted = String.IsNullOrEmpty(input) || inputValidator(input, constraints);
                if (!accepted)
                {
                    Console.WriteLine(errorMessage);
                }
            } while (!accepted);

            return input;
        }

        public static bool NumberInRangeValidator(string value, dynamic arguments)
        {
            int start = arguments.start;
            int end = arguments.end;
            if (int.TryParse(value, out int outValue))
            {
                return outValue >= start && outValue <= end;
            }
            return false;
        }

        public static (ushort row, ushort col) ReadCoordinatesFromNotation(ushort size)
        {
            bool accepted = false;
            ushort row = 0;
            ushort col = 0;

            do
            {
                Console.Write($"Enter position: (A-{(char)('A' + size - 1)})(1-{size}): ");
                var input = Console.ReadLine() ?? "";
                if (String.IsNullOrEmpty(input))
                {
                    accepted = true;
                }
                else if (InRange(input.Length, 2, 3))
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

        public static string ReadSquare(string symbolName, string defaultSymbol)
        {
            bool accepted = false;
            string square = "";
            do
            {
                Console.Write($"Enter character for {symbolName} (default {defaultSymbol}): ");
                string input = Console.ReadLine() ?? "";
                // Counts graphemes instead of length because emojis are sequenses of UTF16 characters.
                var si = new StringInfo(input);
                if (si.LengthInTextElements <= 2)
                {
                    square = si.LengthInTextElements == 0 ? defaultSymbol : input;
                    accepted = true;
                }
                else
                {
                    Console.WriteLine("Invalid entry, enter a _single_ character or leave blank.");
                }
            } while (!accepted);
            return square;
        }

        public static bool SymbolIsWide(string symbol)
        {
            // The number of elements in the enumeration will be the width in UTF16 characters.
            //Console.WriteLine(symbol + " " + symbol.EnumerateRunes().Count() + "Length " + symbol.Length);
            return symbol.EnumerateRunes().Count() > 1 || symbol.Length > 1;
        }
    }
}