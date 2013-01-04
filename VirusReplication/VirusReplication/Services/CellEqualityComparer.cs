using System.Collections.Generic;

namespace VirusReplication.Services {
    public class CellEqualityComparer : IEqualityComparer<Cell> {
        public bool Equals(Cell first, Cell second) {
            return (first.x == second.x) && (first.y == second.y) && (first.state == second.state);
        }

        public int GetHashCode(Cell obj) {
            return obj.x ^ obj.y;
        }
    }
}