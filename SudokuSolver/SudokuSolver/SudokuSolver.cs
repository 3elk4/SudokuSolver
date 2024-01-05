using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Extensions;

namespace SudokuSolver
{
    public class SudokuSolver : ISudokuSolver
    {
        private List<EmptySudokuCell> EmptyCells;

        public SudokuSolver() { }

        public Result<Sudoku> Solve(Sudoku sudoku)
        {
            EmptyCells = PrepareEmptySudokuCells(sudoku).ToList();
            return SolveBacktracing(sudoku);
        }

        private Result<Sudoku> SolveBacktracing(Sudoku sudoku)
        {
            if (EmptyCells.Count == 0) return Result<Sudoku>.Ok(sudoku);

            var cellToCheck = EmptyCells.Last();

            foreach (var value in cellToCheck.PossibleValues)
            {
                if (!IsLegal(sudoku, cellToCheck.Row, cellToCheck.Col, value)) continue;

                sudoku.Grid[cellToCheck.Row, cellToCheck.Col] = value;
                EmptyCells.Remove(cellToCheck);

                // Use recursion to check other empty sudoku cubes
                if (SolveBacktracing(sudoku).Success) return Result<Sudoku>.Ok(sudoku);

                // Undo and check another possible value 
                EmptyCells.Add(cellToCheck);
                sudoku.Grid[cellToCheck.Row, cellToCheck.Col] = 0;

            }
            return Result<Sudoku>.Fail(sudoku, "No solution exists");
        }

        private bool IsLegal(Sudoku sudoku, int rowIdx, int colIdx, int num)
        {
            return !IsNumberAlreadyInRow(sudoku, rowIdx, num) &&
                   !IsNumberAlreadyInCol(sudoku, colIdx, num) &&
                   !IsNumberAlreadyInSquare(sudoku, rowIdx, colIdx, num);
        }

        private bool IsNumberAlreadyInRow(Sudoku sudoku, int rowIdx, int num) {
            return sudoku.Grid.GetRow(rowIdx).Count(n => n == num) > 0;
        }

        private bool IsNumberAlreadyInCol(Sudoku sudoku, int colIdx, int num)
        {
            return sudoku.Grid.GetColumn(colIdx).Count(n => n == num) > 0;
        }

        private bool IsNumberAlreadyInSquare(Sudoku sudoku, int rowIdx, int colIdx, int num)
        {
            int startRow = rowIdx - rowIdx % sudoku.RowSqrt;
            int startCol = colIdx - colIdx % sudoku.ColSqrt;

            for (int i = startRow; i < startRow + sudoku.RowSqrt; i++)
                for (int j = startCol; j < startCol + sudoku.ColSqrt; j++)
                    if (sudoku.Grid[i, j] == num) return true;

            return false;
        }

        private IEnumerable<EmptySudokuCell> PrepareEmptySudokuCells(Sudoku sudoku)
        {
            for (int r = 0; r < sudoku.RowsNumber; r++)
            {
                for (int j = 0; j < sudoku.ColsNumber; j++)
                {
                    if (sudoku.Grid[r, j] != 0) continue;

                    var emptySudokuCell = new EmptySudokuCell(r, j);
                    emptySudokuCell.PossibleValues.AddRange(FindLegalNumbersForPosition(sudoku, r, j));
                    yield return emptySudokuCell;

                }
            }
        }

        private IEnumerable<int> FindLegalNumbersForPosition(Sudoku sudoku, int rowIdx, int colIdx)
        {
            return Enumerable.Range(1, sudoku.RowsNumber).Where(num => IsLegal(sudoku, rowIdx, colIdx, num));
        }
    }
}
