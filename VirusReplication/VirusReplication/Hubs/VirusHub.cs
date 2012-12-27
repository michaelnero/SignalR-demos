﻿using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Hubs;
using VirusReplication.Services;

namespace VirusReplication.Hubs {
    [HubName("virus")]
    public class VirusHub : Hub {
        public void Start(int fps, int q, int x, int y, int n) {
            var calculator = new GenerationCalculator(this.Context.ConnectionId);
            CalculatorRegistry.Register(calculator);

            calculator.Start(fps, q, x, y, n);
        }

        public override Task OnDisconnected() {
            string connectionID = this.Context.ConnectionId;
            return Task.Factory.StartNew(() => CalculatorRegistry.Remove(connectionID));
        }
    }
}