namespace SudokuSolver.Common
{
    interface ISudokuSolver
    {
        public Result<Sudoku> Solve(Sudoku sudoku);
    }
}
