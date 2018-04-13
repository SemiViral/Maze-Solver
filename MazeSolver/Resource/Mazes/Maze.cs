#region usings

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#endregion

namespace MazeFun.Resource.Mazes {
    internal partial class Maze {
        #region MEMBERS

        public Map Map { get; set; }
        private Stopwatch timer;

        #endregion

        public Maze(short boundX, short boundY) {
            Map = new Map(boundX, boundY);

            Console.BackgroundColor = ConsoleColor.White;
            Console.Clear();
        }

        private IEnumerable<Cell> GetNeighbours(Cell cell, int offset) {
            return new List<Cell> {Map.SingleOrDefaultCell(cell.X + offset, cell.Y), Map.SingleOrDefaultCell(cell.X - offset, cell.Y), Map.SingleOrDefaultCell(cell.X, cell.Y + offset), Map.SingleOrDefaultCell(cell.X, cell.Y - offset)}.Where(coord => coord != default(Cell));
        }
    }

    public enum CellArea {
        Edge,
        EdgeNotCorner,
        Corner,
        Inner
    }

    public enum CellType {
        Entry,
        Exit,
        Path,
        Wall,
        Searched,
        All
    }
}
