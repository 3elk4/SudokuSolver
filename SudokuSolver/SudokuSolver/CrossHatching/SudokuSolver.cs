using Microsoft.Toolkit.Extensions;
using SudokuSolver.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.CrossHatching
{
    class SudokuSolver : BaseSudokuSolver
    {
        private Dictionary<int, List<(int i, int j)>> filledPos;
        private Dictionary<int, int> remaining;
        private Dictionary<int, Dictionary<int, List<int>>> graph;

        public SudokuSolver() { }
        public override Result<Sudoku> Solve(Sudoku sudoku)
        {
            BuildFilledPosAndRemaining(sudoku);
            BuildGraph(sudoku);

            remaining = remaining.OrderBy(kvpair => kvpair.Value).ToDictionary(x => x.Key, x => x.Value);

            var remainingKeys = remaining.Keys.ToList();
            int currentRemKey = 0;
            int currentRow = 0;

            Stopwatch sw =Stopwatch.StartNew();
            var result = TrySolve(
                sudoku,
                currentRemKey,
                remainingKeys,
                currentRow,
                graph[remainingKeys[currentRemKey]].Keys.ToList()
            );
            sw.Stop();
            Console.WriteLine(sw.Elapsed);

            return result;
        }

        private Result<Sudoku> TrySolve(Sudoku sudoku, int currentRemKey, List<int> remainingKeys, int currentRow, List<int> rows)
        {
            var value = remainingKeys[currentRemKey];
            var row = rows[currentRow];

            foreach (var col in graph[value][row])
            {
                if (sudoku.Get(row, col) > 0) continue;

                if (IsLegal(sudoku, row, col, value))
                {
                    sudoku.Set(row, col, value);

                    if (currentRow < rows.Count - 1)
                    {
                        if (TrySolve(sudoku, currentRemKey, remainingKeys, currentRow + 1, rows).Success) return Result<Sudoku>.Ok(sudoku);

                        sudoku.Set(row, col, 0);
                        continue;
                    }
                    else
                    {
                        if(currentRemKey >= remainingKeys.Count - 1) return Result<Sudoku>.Ok(sudoku);

                        if (TrySolve(sudoku, currentRemKey + 1, remainingKeys, 0, graph[remainingKeys[currentRemKey + 1]].Keys.ToList()).Success) return Result<Sudoku>.Ok(sudoku);

                        sudoku.Set(row, col, 0);
                        continue;
                    }
                }
            }

            return Result<Sudoku>.Fail(sudoku, "No solution exists");
        }

        private void BuildFilledPosAndRemaining(Sudoku sudoku)
        {
            filledPos = new Dictionary<int, List<(int i, int j)>>();
            remaining = new Dictionary<int, int>();

            var notEmptyCells = sudoku.GetMatchingCells(x => x != 0);

            foreach(var cell in notEmptyCells)
            {
                var value = sudoku.Get(cell);

                if (!filledPos.ContainsKey(value)) filledPos[value] = new List<(int i, int j)>();
                filledPos[value].Add(cell);

                if (!remaining.ContainsKey(value)) remaining[value] = 9;
                remaining[value] -= 1;
            }

            for (int i = 1; i <= Sudoku.MaxValue; i++)
            {
                if (!filledPos.ContainsKey(i)) filledPos[i] = new List<(int i, int j)>();
                if (!remaining.ContainsKey(i)) remaining[i] = 9;
            }
        }

        private void BuildGraph(Sudoku sudoku)
        {
            graph = new Dictionary<int, Dictionary<int, List<int>>>();

            var emptyCells = sudoku.GetMatchingCells(x => x == 0);
            foreach(var kvpair in filledPos)
            {
                var k = kvpair.Key;
                if (!graph.ContainsKey(k)) graph[k] = new Dictionary<int, List<int>>();

                var rows = Enumerable.Range(0, 9).ToList();
                var cols = Enumerable.Range(0, 9).ToList();

                foreach(var v in kvpair.Value)
                {
                    rows.Remove(v.i);
                    cols.Remove(v.j);
                }

                if (rows.Count == 0 || cols.Count == 0) continue;

                foreach(var ec in emptyCells.Where(x => rows.Contains(x.i) && cols.Contains(x.j)))
                {
                    if (!graph[k].ContainsKey(ec.i)) graph[k][ec.i] = new List<int>();
                    graph[k][ec.i].Add(ec.j);
                }
            }
        }
    }
}
