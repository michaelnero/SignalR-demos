using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace VirusReplication.Services {
    [DebuggerDisplay("x = {x}, y = {y}, state = {state}")]
    public class Cell {
        public int x;
        public int y;
        public int state;

        public Cell(int x, int y, int state) {
            this.x = x;
            this.y = y;
            this.state = state;
        }
    }

    public class CellEqualityComparer : IEqualityComparer<Cell> {
        public bool Equals(Cell first, Cell second) {
            return (first.x == second.x) && (first.y == second.y) && (first.state == second.state);
        }

        public int GetHashCode(Cell obj) {
            return obj.x ^ obj.y;
        }
    }
}