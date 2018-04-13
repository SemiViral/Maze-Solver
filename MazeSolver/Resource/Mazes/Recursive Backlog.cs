#region usings

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#endregion

namespace MazeFun.Resource.Mazes {
    internal partial class Maze {
        public void RecursiveBacklog() {
            Map.ResetMap();

            timer = Stopwatch.StartNew();

            Cell currentCell = Map.RandomCellByArea(CellArea.EdgeNotCorner);
            currentCell.Type = CellType.Path;

            Stack<Cell> backlog = new Stack<Cell>();
            backlog.Push(currentCell);

            while (backlog.Count > 0) {
                List<Cell> neighbours;

                while ((neighbours = GetNeighbours(currentCell, 2).Where(cell => cell.Type == CellType.Wall).ToList()).Count == 0 && backlog.Count > 0)
                    currentCell = backlog.Pop();

                if (backlog.Count == 0) {
                    currentCell.Type = CellType.Entry;
                    break;
                }

                // leaving the random generation from 1-neighbours.Count causes
                // too much lateral pathing. Increasing the range of possible
                // number generation allows for more randomness.
                //
                // the `-1` ensures Math.Floor() never encounters a situation in which
                // the random number is a factor of 1000, such as 1000, 2000, 3000, etc.
                // If that were to happen for the highest index of the array, an
                // ArgumentOutOfBounds exception would occur.
                int rand = Program.Rand(1, neighbours.Count * 1000) - 1;

                Debug.WriteLine($"{rand} for length {neighbours.Count}");

                Cell adjacentCell = neighbours[(int)Math.Floor(rand / 1000d)];
                adjacentCell.Type = CellType.Path;

                Map.SingleOrDefaultCell((currentCell.X + adjacentCell.X) / 2, (currentCell.Y + adjacentCell.Y) / 2).Type = CellType.Path;

                backlog.Push(currentCell);

                currentCell = adjacentCell;
            }

            timer.Stop();
            TimeSpan timeSpan = timer.Elapsed;
            Debug.WriteLine($"{timeSpan.Minutes:00} {timeSpan.Seconds:00} {timeSpan.Milliseconds}");
        }
    }
}
