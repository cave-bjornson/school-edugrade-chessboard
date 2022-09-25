using System.Globalization;
using System.Text.RegularExpressions;

namespace ChessBoard
{
    internal static class InputValidation
    {

        /// <summary>
        /// Extensible method for console input and validation.
        /// The method uses callbacks to implement preferred behaviour.
        /// </summary>
        /// <typeparam name="T"> is the type that should be returned.</typeparam>
        /// <param name="inputValidator">is a supplied predicate method that validates according to rules.</param>
        /// <param name="onAccepted"> is a supplied method for conversion of accepted string inputs to type <c>T</c>.</param>
        /// <param name="defaultInput"> is what is supplied in blank inputs.</param>
        /// <param name="message"> is an optional string to be printed before input.</param>
        /// <param name="errorMessage"> is an optional string to print on unaccepted input.</param>
        /// <returns>Value of input possibly converted to type <c>T</c></returns>
        public static T ReadAndValidateInput<T>(
            Predicate<string> inputValidator,
            Func<string, T> onAccepted,
            string defaultInput = "",
            string message = "",
            string errorMessage = "")
        {
            bool accepted;
            string input;
            do
            {
                if (!string.IsNullOrEmpty(message))
                {
                    Console.WriteLine(message);
                }
                input = Console.ReadLine() ?? "";

                if (string.IsNullOrWhiteSpace(input))
                {
                    input = defaultInput;
                }
                accepted = string.IsNullOrEmpty(input) || inputValidator(input);

                if (!accepted && !string.IsNullOrEmpty(errorMessage))
                {
                    Console.WriteLine(errorMessage);
                }
            } while (!accepted);
            return onAccepted(input);
        }


        /// <summary>
        /// Checks if number string is in inclusive range.
        /// </summary>
        /// <param name="value">the string representation of integer</param>
        /// <param name="lower">the lower end of range</param>
        /// <param name="upper">the upper end of range</param>
        /// <returns><c>true</c> if in range</returns>
        public static bool NumberInRangeValidator(string value, int lower, int upper)
        {
            if (int.TryParse(value, out int result))
            {
                return (result >= lower && result <= upper);
            }
            return false;
        }


        /// <summary>
        /// Checks if input is character+number and if inside chessboard.
        /// </summary>
        /// <param name="value">the string to check.</param>
        /// <param name="size">the size of the chessboard.</param>
        /// <returns><c>true</c> if valid eg "A4"</returns>
        public static bool ChessCoordValidator(string value, int size)
        {
            char highestCharUCase = (char)('A' + size - 1);
            char highestCharLCase = (char)('a' + size - 1);
            // First check if input is (upper/lowersize letter)(number)+
            if (Regex.IsMatch(value, @$"^[A-{highestCharUCase}a-{highestCharLCase}]\d+$"))
            {
                var n = int.Parse(value[1..]);
                return (1 <= n && n <= size);
            }
            return false;
        }

        
        /// <summary>
        /// Validates symbol if the input consists of only one unicode <c>Rune</c>
        /// </summary>
        /// <param name="value"></param>
        /// <returns><c>true</c> if just one rune.</returns>
        public static bool SymbolValidator(string value)
        {
            return value.EnumerateRunes().Count() == 1;
            //return new StringInfo(value).LengthInTextElements <= 2;
        }
    }
}