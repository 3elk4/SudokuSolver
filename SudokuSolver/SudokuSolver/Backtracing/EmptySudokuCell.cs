using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Backtracing
{
    public class EmptySudokuCell
    {
        public int Row { get; init; }
        public int Col { get; init; }

        public List<int> PossibleValues { get; init; } = new();
        public EmptySudokuCell(int row, int col)
        {
            Row = row;
            Col = col;
        }
    }
}
