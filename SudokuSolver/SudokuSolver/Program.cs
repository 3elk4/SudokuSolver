using SudokuSolver.Common;
using System;
using System.Diagnostics;

namespace SudokuSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            //int[,] grid = { { 3, 0, 6, 5, 0, 8, 4, 0, 0 },
            //{ 5, 2, 0, 0, 0, 0, 0, 0, 0 },
            //{ 0, 8, 7, 0, 0, 0, 0, 3, 1 },
            //{ 0, 0, 3, 0, 1, 0, 0, 8, 0 },
            //{ 9, 0, 0, 8, 6, 3, 0, 0, 5 },
            //{ 0, 5, 0, 0, 9, 0, 6, 0, 0 },
            //{ 1, 3, 0, 0, 0, 0, 2, 5, 0 },
            //{ 0, 0, 0, 0, 0, 0, 0, 7, 4 },
            //{ 0, 0, 5, 2, 0, 6, 3, 0, 0 }};

            int[,] grid = { 
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 3, 0, 8, 5 },
                { 0, 0, 1, 0, 2, 0, 0, 0, 0 },
                { 0, 0, 0, 5, 0, 7, 0, 0, 0 },
                { 0, 0, 4, 0, 0, 0, 1, 0, 0 },
                { 0, 9, 0, 0, 0, 0, 0, 0, 0 },
                { 5, 0, 0, 0, 0, 0, 0, 7, 3 },
                { 0, 0, 2, 0, 1, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 4, 0, 0, 0, 9 }
            };

            var sudoku = new Sudoku(grid);
            BaseSudokuSolver sudokuSolver;
            var result = Result<Sudoku>.Fail<Sudoku>(null, "Not resolved yet!");

            //cross hatching
            sudokuSolver = new CrossHatching.SudokuSolver();
            result = sudokuSolver.Solve(sudoku.Clone());

            if (result.Success)
                Console.WriteLine(result.Value.ToString());
            else
                Console.WriteLine(result.Error);

            //backtracing
            sudokuSolver = new Backtracing.SudokuSolver();
            result = sudokuSolver.Solve(sudoku.Clone());

            if (result.Success)
                Console.WriteLine(result.Value.ToString());
            else
                Console.WriteLine(result.Error);
        }
    }
}
