using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public class Sudoku
    {
        public int[,] Grid { get; init; }
        public int RowsNumber { get; init; }
        public int ColsNumber { get; init; }
        public int RowSqrt { get; init; }
        public int ColSqrt { get; init; }

        public Sudoku(int[,] grid)
        {
            Grid = grid;
            RowsNumber = grid.GetLength(0);
            ColsNumber = grid.GetLength(1);

            if (RowsNumber != ColsNumber) throw new ArgumentException("Matrix should have the same number of rows and columns.");
            if (Math.Sqrt(RowsNumber) % 1 != 0) throw new ArgumentException("Matrix should be perfect square.");

            RowSqrt = (int)Math.Sqrt(RowsNumber);
            ColSqrt = (int)Math.Sqrt(ColsNumber);
        }

        public override string ToString()
        {
            var horizontalLineLength = ColsNumber + (ColSqrt + 1) * (ColSqrt + 1);
            var horizontalLine = new String('-', horizontalLineLength);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(horizontalLine);

            for (int i = 0; i < RowsNumber; i++)
            {
                for (int j = 0; j < ColsNumber; j++)
                {
                    if (j == 0) sb.Append("|");
                    sb.Append(" ").Append(Grid[i, j]);
                    if ((j + 1) % ColSqrt == 0) sb.Append(" |");
                }
                sb.AppendLine();
                if ((i + 1) % RowSqrt == 0) sb.AppendLine(horizontalLine);
            }
            return sb.ToString();
        }
    }
}
