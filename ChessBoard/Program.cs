namespace ChessBoard
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.WriteLine("◼︎◻︎♛♜♞");
            
            // Interface with the user
            Console.Write("Enter size of chessboard (1-64): ");
            uint size = 0;
            bool acceptedSize = false;
            while (!acceptedSize)
            {
                if (uint.TryParse(Console.ReadLine(), out size))
                {
                    acceptedSize = (size >= 1 && size <= 64);
                    if (acceptedSize)
                    {
                        break;
                    }
                }
                Console.Write("Enter a _number_ between 1 and 64: ");
            }
            Console.WriteLine($"Chessboard Size: {size}x{size}");
        }
    }
}