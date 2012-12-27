namespace StockTrader.Windows.Views.Shell {
    public class ShellPresenter {
        private readonly IShellView view;

        public ShellPresenter(IShellView view) {
            this.view = view;
            this.view.Presenter = this;
        }

        public IShellView View {
            get { return this.view; }
        }
    }
}
