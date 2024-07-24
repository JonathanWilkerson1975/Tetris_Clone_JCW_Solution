using System;
using System.Threading;

class Tetris
{
    static readonly int Width = 10;
    static readonly int Height = 20;
    static readonly char Block = '■';
    static readonly char Empty = ' ';
    static int highScore = 0;
    static int score = 0;

    static int[,] grid = new int[Height, Width];
    static int[,] currentBlock;
    static int blockRow = 0;
    static int blockCol = Width / 2;

    static void Main()
    {
        Console.CursorVisible = false;
        StartMenu();
    }

    static void StartMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("TETRIS");
            Console.WriteLine("1. Start Game");
            Console.WriteLine("2. High Score");
            Console.WriteLine("3. Exit");
            Console.Write("Select an option: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    StartGame();
                    break;
                case "2":
                    ShowHighScore();
                    break;
                case "3":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid choice. Press any key to try again.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    static void StartGame()
    {
        Array.Clear(grid, 0, grid.Length);
        score = 0;
        currentBlock = null;

        while (true)
        {
            if (currentBlock == null)
            {
                currentBlock = GenerateBlock();
                blockRow = 0;
                blockCol = Width / 2;
            }

            if (CanMove(1, 0))
            {
                blockRow++;
            }
            else
            {
                MergeBlock();
                ClearFullRows();
                currentBlock = null;
            }

            DrawGrid();
            Thread.Sleep(500);

            if (IsGameOver())
            {
                Console.Clear();
                Console.WriteLine("Game Over!");
                Console.WriteLine($"Your score: {score}");
                if (score > highScore)
                {
                    highScore = score;
                    Console.WriteLine("New High Score!");
                }
                Console.WriteLine("Press any key to return to the main menu...");
                Console.ReadKey();
                break;
            }
        }
    }

    static int[,] GenerateBlock()
    {
        // Simple 2x2 block
        return new int[,] { { 1, 1 }, { 1, 1 } };
    }

    static bool CanMove(int rowDelta, int colDelta)
    {
        for (int r = 0; r < currentBlock.GetLength(0); r++)
        {
            for (int c = 0; c < currentBlock.GetLength(1); c++)
            {
                if (currentBlock[r, c] != 0)
                {
                    int newRow = blockRow + r + rowDelta;
                    int newCol = blockCol + c + colDelta;

                    if (newRow >= Height || newCol < 0 || newCol >= Width || grid[newRow, newCol] != 0)
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    static void MergeBlock()
    {
        for (int r = 0; r < currentBlock.GetLength(0); r++)
        {
            for (int c = 0; c < currentBlock.GetLength(1); c++)
            {
                if (currentBlock[r, c] != 0)
                {
                    grid[blockRow + r, blockCol + c] = currentBlock[r, c];
                }
            }
        }
    }

    static void ClearFullRows()
    {
        for (int r = Height - 1; r >= 0; r--)
        {
            bool fullRow = true;
            for (int c = 0; c < Width; c++)
            {
                if (grid[r, c] == 0)
                {
                    fullRow = false;
                    break;
                }
            }
            if (fullRow)
            {
                for (int row = r; row > 0; row--)
                {
                    for (int col = 0; col < Width; col++)
                    {
                        grid[row, col] = grid[row - 1, col];
                    }
                }
                for (int col = 0; col < Width; col++)
                {
                    grid[0, col] = 0;
                }
                r++;
                score += 100; // Increase score for each cleared row
            }
        }
    }

    static void DrawGrid()
    {
        Console.Clear();
        for (int r = 0; r < Height; r++)
        {
            for (int c = 0; c < Width; c++)
            {
                if (IsBlock(r, c))
                {
                    Console.Write(Block);
                }
                else
                {
                    Console.Write(grid[r, c] == 0 ? Empty : Block);
                }
            }
            Console.WriteLine();
        }
        Console.WriteLine($"Score: {score}");
        Console.WriteLine($"High Score: {highScore}");
    }

    static bool IsBlock(int row, int col)
    {
        int blockRow = row - Tetris.blockRow;
        int blockCol = col - Tetris.blockCol;

        if (blockRow >= 0 && blockRow < currentBlock.GetLength(0) && blockCol >= 0 && blockCol < currentBlock.GetLength(1))
        {
            return currentBlock[blockRow, blockCol] != 0;
        }
        return false;
    }

    static bool IsGameOver()
    {
        for (int c = 0; c < Width; c++)
        {
            if (grid[0, c] != 0)
            {
                return true;
            }
        }
        return false;
    }

    static void ShowHighScore()
    {
        Console.Clear();
        Console.WriteLine($"High Score: {highScore}");
        Console.WriteLine("Press any key to return to the main menu...");
        Console.ReadKey();
    }
}
