using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    interface ISudokuSolver
    {
        public Result<Sudoku> Solve(Sudoku sudoku);
    }
}
