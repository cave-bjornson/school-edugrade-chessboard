using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBoard
{
    internal class ProgramHelpers
    {


        /// <summary>
        /// The number of elements in the enumeration will be the width in UTF16 characters.
        /// The symbol is "wide" if it's using 2 characters for the glyph.
        /// </summary>
        /// <param name="symbol">is the string with the unicode symbol</param>
        /// <returns></returns>
        public static bool SymbolIsWide(string symbol)
        {
            return symbol.EnumerateRunes().Count() > 1 || symbol.Length > 1;
        }


        /// <summary>
        /// Converts a chess coordinate eg. A1 to corresponding 2D array coordinate eg.[7, 0],
        /// as the 0-row in the array is row 8 on a standard chessboard looking down.
        /// </summary>
        /// <param name="col">is the column character A-Z on a standard board</param>
        /// <param name="row">is the row integer 1-8 on a standard chessboard</param>
        /// <param name="size">is the number of squares on a side of the board</param>
        /// <returns></returns>
        public static (int, int) ChessXYToIndices(char col, int row, int size)
        {
            int arrayRow = size - row;
            int arrayCol = col - 'A';
            return (arrayRow, arrayCol);
        }
    }
}
