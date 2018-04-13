#region usings

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#endregion

namespace MazeFun.Resource.Mazes {
    internal partial class Maze {
        public void Prims() {
            Map.ResetMap();

            timer = Stopwatch.StartNew();

            Cell frontierCell = Map.RandomCellByArea(CellArea.Corner);
            frontierCell.Type = CellType.Path;

            List<Cell> frontiers = new List<Cell>(GetNeighbours(frontierCell, 2).Where(coord => coord.Type == CellType.Wall));

            while (frontiers.Count > 0) {
                frontierCell = frontiers[Program.Rand(frontiers.Count - 1)];

                List<Cell> neighbours = GetNeighbours(frontierCell, 2).Where(cell => cell.Type == CellType.Path).ToList();

                if (neighbours.Count == 0)
                    continue;

                int rand = Program.Rand(1, neighbours.Count * 1000) - 1;

                Cell newPath = neighbours[(int)Math.Floor(rand / 1000d)];

                frontiers.AddRange(GetNeighbours(frontierCell, 2).Where(cell => cell.Type == CellType.Wall));

                frontiers.Remove(frontierCell);

                Map.SingleOrDefaultCell(frontierCell).Type = CellType.Path;
                Map.SingleOrDefaultCell((frontierCell.X + newPath.X) / 2, (frontierCell.Y + newPath.Y) / 2).Type = CellType.Path;
            }

            foreach (Cell cell in Map.Walls) {
                List<Cell> neighbors = GetNeighbours(cell, 1).Where(c => c.Type == CellType.Path).ToList();

                if (neighbors.Count <= 3)
                    continue;

                int rand = Program.Rand(neighbors.Count - 1);
                if (rand < 0)
                    rand = 0;

                neighbors[rand].Type = CellType.Wall;
            }

            timer.Stop();
            TimeSpan timeSpan = timer.Elapsed;
            Debug.WriteLine($"{timeSpan.Minutes:00} {timeSpan.Seconds:00} {timeSpan.Milliseconds}");
        }
    }
}
