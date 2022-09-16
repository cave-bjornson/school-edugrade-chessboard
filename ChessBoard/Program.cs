namespace ChessBoard
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            string whiteSquare = "□";
            string blackSquare = "■";
            string[] gridSquares = { whiteSquare, blackSquare };
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
            var watch = new System.Diagnostics.Stopwatch();

            string[] chessBoard = new string[size * size];
            Array.Fill(chessBoard, blackSquare);
            for (int i = 0; i < chessBoard.Length; i += 2)
            {
                chessBoard[i] = whiteSquare;
            }
            watch.Start();
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Console.Write(chessBoard[i + j]);
                }
                Console.WriteLine();
            }
            watch.Stop();

            Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
            watch.Reset();
            watch.Start();

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Console.Write(gridSquares[(i + j) % 2]);
                }
                Console.WriteLine();
            }
            watch.Stop();

            Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
            watch.Reset();
            watch.Start();
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if ((i + j) % 2 == 0)
                    {
                        Console.Write(whiteSquare);
                    }
                    else
                    {
                        Console.Write(blackSquare);
                    }
                }
                Console.WriteLine();
            }
            watch.Stop();

            Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
            //int[,] array2D = new int[,] { { 1, 2 }, { 3, 4 }, { 5, 6 }, { 7, 8 } };
        }
    }
}