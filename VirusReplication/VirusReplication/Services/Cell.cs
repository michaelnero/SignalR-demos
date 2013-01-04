using System.Diagnostics;

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
}