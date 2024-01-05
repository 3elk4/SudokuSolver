using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public class Result
    {
        public bool IsSolved { get; init; }
        public Sudoku Sudoku { get; init; }

        public Result(bool isSolved, Sudoku sudoku)
        {
            IsSolved = isSolved;
            Sudoku = sudoku;
        }
    }
}
