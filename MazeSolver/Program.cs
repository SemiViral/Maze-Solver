#region usings

using System;
using MazeFun.Resource.Mazes;

#endregion

namespace MazeFun {
    internal class Program {
        #region MEMBERS

        private static Maze _maze;

        private static readonly Random _rand = new Random();
        private static readonly object _objLock = new object();

        #endregion

        private static void Main(string[] args) {
            Console.ReadLine();

            while (true) {
                _maze = new Maze(55, 55);

                _maze.Map.ResetMap();
                _maze.RecursiveBacklog();
                _maze.Map.PrintMap();

                Console.ReadKey();

                _maze.Map.ResetMap();
                _maze.Prims();
                _maze.Map.PrintMap();

                Console.ReadKey();
            }
        }

        public static int Rand(int maxValue) {
            lock (_objLock) {
                return _rand.Next(maxValue);
            }
        }

        public static int Rand(int minValue, int maxValue) {
            lock (_objLock) {
                return _rand.Next(minValue, maxValue);
            }
        }
    }
}
