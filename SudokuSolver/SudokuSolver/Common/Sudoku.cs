﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.Common
{
    public class Sudoku
    {
        public static int MaxValue = 9;
        public int[,] Grid { get; init; }
        public int RowsNumber { get; init; }
        public int ColsNumber { get; init; }
        public int ValSqrt { get; init; }

        public Sudoku(int[,] grid)
        {
            Grid = grid;
            RowsNumber = grid.GetLength(0);
            ColsNumber = grid.GetLength(1);

            if (RowsNumber != MaxValue || ColsNumber != MaxValue) throw new ArgumentException("Matrix should be 9x9 square.");
            //if (RowsNumber != ColsNumber) throw new ArgumentException("Matrix should have the same number of rows and columns.");
            //if (Math.Sqrt(RowsNumber) % 1 != 0) throw new ArgumentException("Matrix should be perfect square.");

            ValSqrt = (int)Math.Sqrt(RowsNumber);
        }

        public int Get(int i, int j) => Grid[i, j];
        public int Get((int i, int j) x) => Grid[x.i, x.j];
        public void Set(int i, int j, int k) => Grid[i, j] = k;
        public void Set((int i, int j) x, int k) => Grid[x.i, x.j] = k;

        public IEnumerable<(int i, int j)> GetMatchingCells(Func<int, bool> predicate)
        {
            for (int i = 0; i < RowsNumber; ++i)
            {
                for (int j = 0; j < ColsNumber; ++j)
                {
                    if (predicate(Grid[i, j])) yield return (i, j);
                }
            }
        }

        public Sudoku Clone()
        {
            return new Sudoku((int[,])Grid.Clone());
        }

        public override string ToString()
        {
            var horizontalLineLength = ColsNumber + (ValSqrt + 1) * (ValSqrt + 1);
            var horizontalLine = new String('-', horizontalLineLength);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(horizontalLine);

            for (int i = 0; i < RowsNumber; i++)
            {
                for (int j = 0; j < ColsNumber; j++)
                {
                    if (j == 0) sb.Append("|");
                    sb.Append(" ").Append(Grid[i, j]);
                    if ((j + 1) % ValSqrt == 0) sb.Append(" |");
                }
                sb.AppendLine();
                if ((i + 1) % ValSqrt == 0) sb.AppendLine(horizontalLine);
            }
            return sb.ToString();
        }
    }
}
