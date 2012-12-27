using Microsoft.Practices.Prism.Events;

namespace StockTrader.Windows.Events {
    public class ConnectionStateChangedEventArgs {
        public ConnectionStateChangedEventArgs(bool isConnected, bool hasFault) {
            this.IsConnected = isConnected;
            this.HasFault = hasFault;
        }

        public bool IsConnected { get; private set; }

        public bool HasFault { get; private set; }
    }

    public class ConnectionStateChangedEvent : CompositePresentationEvent<ConnectionStateChangedEventArgs> {
         
    }
}