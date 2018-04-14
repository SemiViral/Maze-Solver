#region usings

using System;
using MazeFun.Resource.Mazes;

#endregion

namespace MazeFun.Resource {
    internal class Cell {
        #region MEMBERS

        /// <summary>
        ///     This will be the width of each cell in
        ///     console units.
        /// </summary>
        private const int DISPLAY_SIZE_MULTIPLIER = 3;

        public int X { get; set; }

        public int Y { get; set; }

        public ConsoleColor Color => ColorByType(Type);

        public CellType Type { get; set; }

        #endregion

        public Cell(int x, int y, CellType type = CellType.Wall) {
            X = x;
            Y = y;
            Type = type;
        }

        public override string ToString() {
            return new string(' ', DISPLAY_SIZE_MULTIPLIER);
            //return $" {X:00} {Y:00} ";
        }

        public void Print() {
            Console.ForegroundColor = ConsoleColor.DarkCyan;

            Console.SetCursorPosition(X * DISPLAY_SIZE_MULTIPLIER, Y);
            Console.BackgroundColor = Color;
            Console.Write(ToString());
        }

        public static ConsoleColor ColorByType(CellType type) {
            switch (type) {
                case CellType.Path:
                    return ConsoleColor.Black;
                case CellType.Wall:
                    return ConsoleColor.White;
                case CellType.Searched:
                    return ConsoleColor.Red;
                case CellType.Entry:
                    return ConsoleColor.Green;
                case CellType.Exit:
                    return ConsoleColor.DarkRed;
                case CellType.All:
                    throw new ArgumentOutOfRangeException();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
