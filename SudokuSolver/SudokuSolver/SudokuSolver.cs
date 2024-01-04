using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Extensions;

namespace SudokuSolver
{
    public class SudokuSolver
    {
        public class EmptySudokuCube
        {
            public int Row { get; init; }
            public int Col { get; init; }

            public List<int> PossibleValues { get; init; }
            public EmptySudokuCube(int row, int col)
            {
                Row = row;
                Col = col;
                PossibleValues = new List<int>();
            }
        }

        private int[,] _grid;
        private int _maxRows;
        private int _maxCols;
        private int _rowSqrt;
        private int _colSqrt;

        private List<EmptySudokuCube> _emptyCubes;

        private void InitEmptyCubes()
        {
            _emptyCubes = new List<EmptySudokuCube>();

            for (int i = 0; i < _maxRows; i++)
            {
                for (int j = 0; j < _maxCols; j++)
                {
                    if (_grid[i, j] == 0)
                    {
                        var eSC = new EmptySudokuCube(i, j);
                        // Check all legal nums and add them to cube's possible values
                        eSC.PossibleValues.AddRange(
                            Enumerable.Range(1, _maxRows).Where(num => IsLegal(i, j, num)).ToList()
                        );
                        _emptyCubes.Add(eSC);
                    }
                }
            }
        }

        public SudokuSolver(int [, ] grid)
        {
            _grid = grid;
            _maxRows = grid.GetLength(0);
            _maxCols = grid.GetLength(1);

            if (_maxCols != _maxRows) throw new ArgumentException("Matrix should have the same number of rows and columns.");

            _rowSqrt = (int)Math.Sqrt(_maxRows);
            _colSqrt = (int)Math.Sqrt(_maxCols);

            InitEmptyCubes();
        }

        public bool Solve()
        {
            if (_emptyCubes.Count() == 0) return true;

            // Take empty cube with the least possibilities values 
            var eSC = _emptyCubes.OrderByDescending(eSC => eSC.PossibleValues.Count()).Last();

            foreach (var value in eSC.PossibleValues)
            {
                if (IsLegal(eSC.Row, eSC.Col, value))
                {
                    _grid[eSC.Row, eSC.Col] = value;
                    _emptyCubes.Remove(eSC);

                    // Use recursion to check other empty sudoku cubes
                    if (Solve()) return true;

                    // Undo and check another possible value 
                    _emptyCubes.Add(eSC);
                    _grid[eSC.Row, eSC.Col] = 0;
                }
            }
            return false;
        }

        private bool IsLegal(int rowIdx, int colIdx, int num)
        {
            var row = _grid.GetRow(rowIdx);
            var col = _grid.GetColumn(colIdx);

            // Check if we find the same num in the similar row
            if (row.Count(n => n == num) > 0) return false;

            // Check if we find the same num in the similar column
            if (col.Count(n => n == num) > 0) return false;

            // Check if we find the same num in the particular 3*3 matrix
            int startRow = rowIdx - rowIdx % _rowSqrt;
            int startCol = colIdx - colIdx % _colSqrt;

            for (int i = startRow; i < startRow + _rowSqrt; i++)
                for (int j = startCol; j < startCol + _colSqrt; j++)
                    if (_grid[i, j] == num) return false;

            return true;
        }

        public override string ToString()
        {
            var horLine = new String('-', _maxCols + (int)Math.Pow(_colSqrt + 1, 2));

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(horLine);

            for (int i = 0; i < _maxRows; i++)
            {
                for (int j = 0; j < _maxCols; j++)
                {
                    if(j == 0) sb.Append("|");
                    sb.Append(" " + _grid[i, j]);
                    if ((j + 1) % _colSqrt == 0) sb.Append(" |");
                }
                sb.AppendLine();
                if ((i + 1) % _rowSqrt == 0) sb.AppendLine(horLine);
            }
            return sb.ToString();
        }
    }
}
