using System;

class Program
{
    const int SIZE = 8;
    static char[,] board = new char[SIZE, SIZE];
    static bool[,] attacked = new bool[SIZE, SIZE];
    static int[] knightMovesX = { -2, -1, 1, 2, 2, 1, -1, -2 };
    static int[] knightMovesY = { 1, 2, 2, 1, -1, -2, -2, -1 };

    static void Main()
    {
        
        for (int i = 0; i < SIZE; i++)
            for (int j = 0; j < SIZE; j++)
                board[i, j] = '.';

        Console.Write("Enter starting X (0-7): ");
        int startX = int.Parse(Console.ReadLine());

        Console.Write("Enter starting Y (0-7): ");
        int startY = int.Parse(Console.ReadLine());


        PlaceKentavr(startX, startY);

        while (true)
        {
            var nextMove = FindBestMove();
            if (nextMove == null) break;

            PlaceKentavr(nextMove.Value.x, nextMove.Value.y);
        }

        Console.WriteLine("\nFinal Board:");
        DisplayFinalBoard();
    }

    static void PlaceKentavr(int x, int y)
    {
        board[x, y] = 'K';
        attacked[x, y] = true;

        for (int i = 0; i < SIZE; i++)
        {
            attacked[x, i] = true;
            attacked[i, y] = true;
        }

    
        for (int i = 0; i < 8; i++)
        {
            int nx = x + knightMovesX[i];
            int ny = y + knightMovesY[i];
            if (IsInside(nx, ny))
                attacked[nx, ny] = true;
        }

        Console.WriteLine($"\nPlaced Kentavr at ({x}, {y})");
        DisplayHeuristicBoard();
    }

    static (int x, int y)? FindBestMove()
    {
        int maxRemaining = -1;
        (int x, int y)? bestMove = null;

        for (int i = 0; i < SIZE; i++)
        {
            for (int j = 0; j < SIZE; j++)
            {
                if (attacked[i, j]) continue;

                int remaining = SimulatePlacement(i, j);
                if (remaining > maxRemaining)
                {
                    maxRemaining = remaining;
                    bestMove = (i, j);
                }
            }
        }

        return bestMove;
    }

    static int SimulatePlacement(int x, int y)
    {
        bool[,] tempAttacked = (bool[,])attacked.Clone();

        for (int i = 0; i < SIZE; i++)
        {
            tempAttacked[x, i] = true;
            tempAttacked[i, y] = true;
        }

        for (int i = 0; i < 8; i++)
        {
            int nx = x + knightMovesX[i];
            int ny = y + knightMovesY[i];
            if (IsInside(nx, ny))
                tempAttacked[nx, ny] = true;
        }

        tempAttacked[x, y] = true;

        int count = 0;
        for (int i = 0; i < SIZE; i++)
            for (int j = 0; j < SIZE; j++)
                if (!tempAttacked[i, j])
                    count++;

        return count;
    }

    static void DisplayHeuristicBoard()
    {
        Console.WriteLine("\nBoard with Heuristic Values:");
        for (int i = 0; i < SIZE; i++)
        {
            for (int j = 0; j < SIZE; j++)
            {
                if (board[i, j] == 'K')
                    Console.Write(" K ");
                else if (attacked[i, j])
                    Console.Write(" . ");
                else
                    Console.Write($" {SimulatePlacement(i, j)} ");
            }
            Console.WriteLine();
        }
    }

    static void DisplayFinalBoard()
    {
        for (int i = 0; i < SIZE; i++)
        {
            for (int j = 0; j < SIZE; j++)
            {
                Console.Write(board[i, j] == 'K' ? " K " : " . ");
            }
            Console.WriteLine();
        }
    }

    static bool IsInside(int x, int y)
    {
        return x >= 0 && x < SIZE && y >= 0 && y < SIZE;
    }
}
