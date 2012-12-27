using Microsoft.Practices.Prism.Events;

namespace StockTrader.Windows.Events {
    public class BalanceRequestedEventArgs {
    }

    public class BalanceRequestedEvent : CompositePresentationEvent<BalanceRequestedEventArgs> {
         
    }
}