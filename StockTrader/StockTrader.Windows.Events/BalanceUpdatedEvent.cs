using Microsoft.Practices.Prism.Events;

namespace StockTrader.Windows.Events {
    public class BalanceUpdatedEventArgs {
        public BalanceUpdatedEventArgs(string accountID, decimal balance) {
            this.AccountID = accountID;
            this.Balance = balance;
        }

        public string AccountID { get; private set; }

        public decimal Balance { get; private set; }
    }

    public class BalanceUpdatedEvent : CompositePresentationEvent<BalanceUpdatedEventArgs> {
         
    }
}