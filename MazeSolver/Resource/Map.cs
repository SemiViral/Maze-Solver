#region usings

using System;
using System.Collections.Generic;
using System.Linq;
using MazeFun.Resource.Mazes;

#endregion

namespace MazeFun.Resource {
    internal class Map {
        #region MEMBERS

        public int BoundX => _cells.GetLength(0);
        public int BoundY => _cells.GetLength(1);

        public IEnumerable<Cell> Walls => _cells.Cast<Cell>().Where(coord => coord.Type == CellType.Wall);

        public IEnumerable<Cell> Paths => _cells.Cast<Cell>().Where(coord => coord.Type == CellType.Path);

        private readonly Cell[,] _cells;

        #endregion

        public Map(short boundX, short boundY) {
            _cells = new Cell[boundX, boundY];

            IterateCellMapWithExecution((i, j) => _cells[i, j] = new Cell(i, j));
        }

        public Cell SingleOrDefaultCell(int x, int y) {
            if (x < 0 || y < 0 || x > BoundX - 1 || y > BoundY - 1 || _cells[x, y] == null)
                return default(Cell);

            return _cells[x, y];
        }

        public Cell SingleOrDefaultCell(Cell coord) {
            if (coord == default(Cell) || coord.X > BoundX || coord.Y > BoundY || _cells[coord.X, coord.Y] == null)
                return default(Cell);

            return _cells[coord.X, coord.Y];
        }

        public Cell RandomCellOrDefault(CellType type) {
            if (type == CellType.All)
                return _cells.Cast<Cell>().ToList()[Program.Rand(1, _cells.Length) - 1];

            List<Cell> randCells = _cells.Cast<Cell>().Where(cell => cell.Type == type).ToList();

            return randCells.Count > 0 ? randCells[Program.Rand(randCells.Count - 1)] : default(Cell);
        }

        public Cell RandomCellByArea(CellArea area) {
            Func<Cell, bool> continueLooping;

            switch (area) {
                case CellArea.Corner:
                    continueLooping = cell => !IsCorner(cell);
                    break;
                case CellArea.Edge:
                    continueLooping = cell => !IsEdge(cell);
                    break;
                case CellArea.EdgeNotCorner:
                    continueLooping = cell => IsCorner(cell) || !IsEdge(cell);
                    break;
                case CellArea.Inner:
                    continueLooping = cell => IsCorner(cell) || IsEdge(cell);
                    break;
                default:
                    continueLooping = cell => false;
                    break;
            }

            Cell returnCell;

            while (continueLooping(returnCell = RandomCellOrDefault(CellType.All))) { }

            return returnCell;
        }

        /// <summary>
        ///     Determines whether the given coordinate is on the edge of the map
        /// </summary>
        private bool IsEdge(Cell cell) {
            return cell.Y == 0 || cell.X == 0 || cell.X == BoundX - 1 || cell.Y == BoundY - 1;
        }

        /// <summary>
        ///     Determines whether the given coordinate is a corner
        /// </summary>
        private bool IsCorner(Cell cell) {
            return cell.X == 0 && cell.Y == 0 || cell.X == 0 && cell.Y == BoundX - 1 || cell.Y == 0 && cell.X == BoundY - 1 || cell.Y == BoundX - 1 && cell.X == BoundY - 1;
        }

        /// <summary>
        ///     Determines whether the given coordinates share a side or overlap
        /// </summary>
        private static bool IsSharingSides(Cell cell1, Cell cell2) {
            if (cell1.X == cell2.X)
                return cell1.Y == cell2.Y || cell1.Y == cell2.Y - 1 || cell1.Y == cell2.Y + 1;

            if (cell1.Y == cell2.Y)
                return cell1.X == cell2.X || cell1.X == cell2.X - 1 || cell1.X == cell2.X + 1;

            return false;
        }

        /// <summary>
        ///     Iterates over each cell in the map and executes an Action
        /// </summary>
        /// <param name="execution">action (int x, int y) to execute on cells</param>
        public void IterateCellMapWithExecution(Action<int, int> execution) {
            for (int x = 0; x < _cells.GetLength(0); x++)
            for (int y = 0; y < _cells.GetLength(1); y++)
                execution.Invoke(x, y);
        }

        public void PrintMap() {
            IterateCellMapWithExecution((x, y) => _cells[x, y].Print());
        }

        public void ResetMap() {
            IterateCellMapWithExecution((x, y) => _cells[x, y].Type = CellType.Wall);
        }
    }
}
