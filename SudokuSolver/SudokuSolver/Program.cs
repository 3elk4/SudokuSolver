using System;
using System.Diagnostics;

namespace SudokuSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            int[,] grid = { { 3, 0, 6, 5, 0, 8, 4, 0, 0 },
            { 5, 2, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 8, 7, 0, 0, 0, 0, 3, 1 },
            { 0, 0, 3, 0, 1, 0, 0, 8, 0 },
            { 9, 0, 0, 8, 6, 3, 0, 0, 5 },
            { 0, 5, 0, 0, 9, 0, 6, 0, 0 },
            { 1, 3, 0, 0, 0, 0, 2, 5, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 7, 4 },
            { 0, 0, 5, 2, 0, 6, 3, 0, 0 }};

            var sudoku = new Sudoku(grid);
            var sudokuSolver = new SudokuSolver();
            Stopwatch sw = new Stopwatch();
            var result = sudokuSolver.Solve(sudoku);

            if (result.IsSolved)
                Console.WriteLine(result.Sudoku.ToString());
            else
                Console.WriteLine("No Solution exists");
        }
    }
}
