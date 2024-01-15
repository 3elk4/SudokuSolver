using Microsoft.Toolkit.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Common
{
    public abstract class BaseSudokuSolver : ISudokuSolver
    {
        protected bool IsLegal(Sudoku sudoku, int rowIdx, int colIdx, int num)
        {
            return !IsNumberAlreadyInRow(sudoku, rowIdx, num) &&
                   !IsNumberAlreadyInCol(sudoku, colIdx, num) &&
                   !IsNumberAlreadyInSquare(sudoku, rowIdx, colIdx, num);
        }

        private bool IsNumberAlreadyInRow(Sudoku sudoku, int rowIdx, int num)
        {
            return sudoku.Grid.GetRow(rowIdx).Count(n => n == num) > 0;
        }

        private bool IsNumberAlreadyInCol(Sudoku sudoku, int colIdx, int num)
        {
            return sudoku.Grid.GetColumn(colIdx).Count(n => n == num) > 0;
        }

        private bool IsNumberAlreadyInSquare(Sudoku sudoku, int rowIdx, int colIdx, int num)
        {
            int startRow = rowIdx - rowIdx % sudoku.ValSqrt;
            int startCol = colIdx - colIdx % sudoku.ValSqrt;

            for (int i = startRow; i < startRow + sudoku.ValSqrt; i++)
                for (int j = startCol; j < startCol + sudoku.ValSqrt; j++)
                    if (sudoku.Grid[i, j] == num) return true;

            return false;
        }

        public abstract Result<Sudoku> Solve(Sudoku sudoku);
    }
}
