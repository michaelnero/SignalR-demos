using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using VirusReplication.Hubs;

namespace VirusReplication.Services {
    public class GenerationCalculator {
        private static readonly IHubContext context = GlobalHost.ConnectionManager.GetHubContext<VirusHub>();

        private readonly object sync = new object();
        private readonly string connectionID;

        private HighFrequencyTimer timer;
        private Cell[,] cells;
        private int initialState;
        private int width;
        private int height;
        private int generations;

        public GenerationCalculator(string connectionID) {
            this.connectionID = connectionID;
        }

        public string ConnectionID {
            get { return this.connectionID; }
        }

        public void Start(double fps, int q, int x, int y, int n) {
            lock (this.sync) {
                if (null == this.timer) {
                    this.Initialize(fps, q, x, y, n);
                }
            }

            this.timer.Start();
        }

        public void Stop() {
            lock (this.sync) {
                if (null != this.timer) {
                    this.timer.Stop();
                    this.timer = null;

                    this.cells = null;
                }
            }
        }

        private void OnCallback(long frameID) {
            var updatedCells = new HashSet<Cell>(new CellEqualityComparer());

            for (int i = 0; i < this.width; i++) {
                for (int j = 0; j < this.height; j++) {
                    var cell = this.cells[i, j];
                    if (cell.state != this.initialState) {
                        if (updatedCells.Contains(cell)) {
                            continue;
                        }

                        if (this.InfectCell(cell)) {
                            updatedCells.Add(cell);
                        }

                        // This cell is infected, so infect one random neighboring cell

                        var neighbor = this.GetNeighborAt(cell, RandomNumber.GetRandomInt(8));
                        if (null == neighbor) {
                            continue;
                        }

                        if (updatedCells.Contains(neighbor)) {
                            continue;
                        }

                        if (this.InfectCell(neighbor)) {
                            updatedCells.Add(neighbor);
                        }
                    }
                }
            }

            if (0 != updatedCells.Count) {
                context.Clients.Client(this.connectionID).updateGrid(updatedCells.ToArray());
            }

            int currentGeneration = Interlocked.Decrement(ref this.generations);
            if (0 >= currentGeneration) {
                this.Stop();
            }
        }

        private void OnStarted() {
        }

        private void OnStopped() {
            context.Clients.Client(this.connectionID).notifyDone();
            CalculatorRegistry.Remove(this.connectionID);
        }

        private void OnActualFpsUpdate(int fps) {
        }

        private void Initialize(double fps, int q, int x, int y, int n) {
            // Setup the timer
            this.timer = new HighFrequencyTimer(fps, this.OnCallback, this.OnStarted, this.OnStopped, this.OnActualFpsUpdate);

            // Setup cells
            this.cells = new Cell[x, y];
            for (int i = 0; i < x; i++) {
                for (int j = 0; j < y; j++) {
                    this.cells[i, j] = new Cell(i, j, q);
                }
            }

            // Infect a random cell in the system
            int randomX = RandomNumber.GetRandomInt(x);
            int randomY = RandomNumber.GetRandomInt(y);

            var cell = this.cells[randomX, randomY];
            this.InfectCell(cell);

            this.initialState = q;
            this.width = x;
            this.height = y;
            this.generations = n;
        }

        private bool InfectCell(Cell cell) {
            if (cell.state > 0) {
                cell.state--;
                return true;
            }

            return false;
        }

        private Cell GetNeighborAt(Cell cell, int index) {
            index = index % 8;
            int targetx = cell.x;
            int targety = cell.y;

            switch (index) {
                case 0:
                    targetx--;
                    break;
                case 1:
                    targetx--;
                    targety--;
                    break;
                case 2:
                    targety--;
                    break;
                case 3:
                    targety--;
                    targetx++;
                    break;
                case 4:
                    targetx++;
                    break;
                case 5:
                    targetx++;
                    targety++;
                    break;
                case 6:
                    targety++;
                    break;
                case 7:
                    targety++;
                    targetx--;
                    break;
            }

            if ((targetx > (this.width - 1)) || (targety > (this.height - 1)) || (0 > targetx) || (0 > targety)) {
                return null;
            }

            var neighbor = this.cells[targetx, targety];
            return neighbor;
        }
    }
}