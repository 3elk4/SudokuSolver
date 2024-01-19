using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SudokuSolver.Common;

namespace SudokuSolver.Backtracing
{
    public class SudokuSolver : BaseSudokuSolver
    {
        private List<EmptySudokuCell> EmptyCells;

        public SudokuSolver() { }

        public override Result<Sudoku> Solve(Sudoku sudoku)
        {
            EmptyCells = PrepareEmptySudokuCells(sudoku).ToList();

            
            Stopwatch sw = Stopwatch.StartNew();
            var result = TrySolve(sudoku);
            sw.Stop();
            Console.WriteLine(sw.Elapsed);

            return result;
        }

        private Result<Sudoku> TrySolve(Sudoku sudoku)
        {
            if (EmptyCells.Count == 0) return Result<Sudoku>.Ok(sudoku);

            var cellToCheck = EmptyCells.Last();

            foreach (var value in cellToCheck.PossibleValues)
            {
                if (!IsLegal(sudoku, cellToCheck.Row, cellToCheck.Col, value)) continue;

                sudoku.Grid[cellToCheck.Row, cellToCheck.Col] = value;
                EmptyCells.Remove(cellToCheck);

                // Use recursion to check other empty sudoku cubes
                if (TrySolve(sudoku).Success) return Result<Sudoku>.Ok(sudoku);

                // Undo and check another possible value 
                EmptyCells.Add(cellToCheck);
                sudoku.Grid[cellToCheck.Row, cellToCheck.Col] = 0;

            }
            return Result<Sudoku>.Fail(sudoku, "No solution exists");
        }

        private IEnumerable<EmptySudokuCell> PrepareEmptySudokuCells(Sudoku sudoku)
        {
            foreach(var emptyCell in sudoku.GetMatchingCells(x => x == 0))
            {
                var emptySudokuCell = new EmptySudokuCell(emptyCell.i, emptyCell.j);
                emptySudokuCell.PossibleValues.AddRange(FindLegalNumbersForPosition(sudoku, emptyCell.i, emptyCell.j));
                yield return emptySudokuCell;
            }
        }

        private IEnumerable<int> FindLegalNumbersForPosition(Sudoku sudoku, int rowIdx, int colIdx)
        {
            return Enumerable.Range(1, Sudoku.MaxValue).Where(num => IsLegal(sudoku, rowIdx, colIdx, num));
        }
    }
}
