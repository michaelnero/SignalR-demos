/// <reference path="jquery-1.8.3.min.js" />
/// <reference path="jquery.signalR-1.0.0-alpha2.min.js" />
/// <reference path="bootstrap.min.js" />
/// <reference path="knockout-2.2.0.debug.js" />

Number.prototype.formatMoney = function (c, d, t) {
    var n = this, c = isNaN(c = Math.abs(c)) ? 2 : c, d = d == undefined ? "," : d, t = t == undefined ? "." : t, s = n < 0 ? "-" : "", i = parseInt(n = Math.abs(+n || 0).toFixed(c)) + "", j = (j = i.length) > 3 ? j % 3 : 0;
    return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? d + Math.abs(n - i).toFixed(c).slice(2) : "");
};

var App = (function () {
    'use strict';
    
    // The StockHub SignalR hub reference
    var stocksHub = null;
    
    function formatCurrency(amount) {
        return amount ? '$' + amount.formatMoney(2, '.', ',') : '$ ---';
    }
    
    function subscribedStock(symbol, price, quantity, cssClass) {
        var self = this;

        this.symbol = ko.observable(symbol);
        this.price = ko.observable(price);
        this.previousPrice = ko.observable(null);
        this.cssClass = ko.observable(cssClass);
        this.quantity = ko.observable(quantity || 50);
        this.movementCssClass = ko.observable();

        this.timeoutID = null;

        this.formattedPrice = ko.computed(function () {
            return formatCurrency(self.price());
        }, this);

        this.formattedPreviousPrice = ko.computed(function () {
            return formatCurrency(self.previousPrice());
        }, this);
    }

    function historyItem() {
        var self = this;

        this.requestID = ko.observable();
        this.status = ko.observable();
        this.action = ko.observable();
        this.symbol = ko.observable();
        this.quantity = ko.observable();
        this.price = ko.observable();
        this.amount = ko.observable();

        this.formattedPrice = ko.computed(function () {
            return formatCurrency(self.price());
        }, this);

        this.formattedAmount = ko.computed(function () {
            return formatCurrency(self.amount());
        }, this);

        this.formattedStatus = ko.computed(function () {
            var evaluatedStatus = self.status();
            if (0 == evaluatedStatus) {
                return 'submitted';
            } else if (1 == evaluatedStatus) {
                return 'completed';
            } else {
                return 'rejected';
            }
        }, this);

        this.formattedAction = ko.computed(function () {
            var evaluatedAction = self.action();
            if (0 == evaluatedAction) {
                return 'BUY';
            } else {
                return 'SELL';
            }
        }, this);
    }

    function viewModel() {
        var root = this;
        
        this.startUp = new function () {
            var self = this;

            this.accountID = ko.observable();
            this.inAccountIDEntryState = ko.observable(true);
            this.inConnectingState = ko.observable(false);

            this.start = function () {
                $('#initialization-modal').modal('show');
            };

            this.onContinue = function () {
                if (self.accountID()) {
                    self.inAccountIDEntryState(false);
                    self.inConnectingState(true);

                    stocksHub.state.AccountID = self.accountID();

                    $.connection.hub.start().done(function () {
                        stocksHub.server.requestAccountBalance().done(function() {
                            $('#initialization-modal').modal('hide');
                        });
                    });
                }
            };
        };

        this.balance = new function () {
            var self = this;
            
            this.accountID = ko.observable();
            this.balance = ko.observable();
            this.stocks = ko.observableArray([]);
            
            this.formattedBalance = ko.computed(function () {
                return formatCurrency(self.balance());
            }, this);
            
            this.onShowBalances = function () {
                $('#stock-quantities-modal').modal('show');
            };

            this.onBalanceUpdated = function (accountID, balance, stocks) {
                self.accountID(accountID);
                self.balance(balance);

                var allStocks = self.stocks();

                for (var i = 0; i < stocks.length; i++) {
                    var stock = stocks[i];

                    var stockQuantity = ko.utils.arrayFirst(allStocks, function(item) {
                        return (item.symbol() == stock.symbol);
                    });
                    
                    if (stockQuantity) {
                        stockQuantity.quantity(stock.quantity);
                    } else {
                        stockQuantity = {
                            symbol: ko.observable(stock.symbol),
                            quantity: ko.observable(stock.quantity)
                        };

                        self.stocks.unshift(stockQuantity);
                    }
                }
            };
        };

        this.history = new function () {
            var self = this;
            
            this.history = ko.observableArray([]);
            
            this.historyCount = ko.computed(function () {
                return self.history().length;
            }, this);
            
            this.onShowHistory = function () {
                $('#history-modal').modal('show');
            };

            this.onStockActionExecuted = function (requestID, status, action, symbol, quantity, price, amount) {
                var allItems = self.history();
                var item = ko.utils.arrayFirst(allItems, function (element) {
                    return (element.requestID() == requestID);
                });
                
                if (!item) {
                    item = new historyItem();
                    self.history.unshift(item);
                }

                item.requestID(requestID);
                item.status(status);
                item.action(action);
                item.symbol(symbol);
                item.quantity(quantity);
                item.price(price);
                item.amount(amount);
            };
        };

        this.trader = new function () {
            var stockPanelCssClasses = ['stock-trader green button', 'stock-trader blue button', 'stock-trader red button', 'stock-trader magenta button', 'stock-trader orange button', 'stock-trader orangellow button'];
            
            var self = this;
            var currentCssClassIndex = 0;

            this.stocks = ko.observableArray([]);
            
            this.onSubscribe = function (data, event) {
                if (13 == event.keyCode) {
                    var element = event.srcElement;

                    var input = element.value;
                    if (input) {
                        var tokens = ko.utils.stringTokenize(input, ',');
                        if (1 <= tokens.length) {
                            var symbol = tokens[0].toUpperCase();

                            var quantity = null;
                            if (2 <= tokens.length) {
                                quantity = tokens[1];
                            }

                            stocksHub.server.subscribeToStock(symbol);

                            var stockCssClass = stockPanelCssClasses[currentCssClassIndex++ % 6];
                            self.stocks.push(new subscribedStock(symbol, null, quantity, stockCssClass));
                        }
                    }

                    element.value = null;
                }
            };
            
            this.onTileLeftClick = function (stock, event) {
                if (event.ctrlKey) {
                    unsubscribe(stock);
                } else {
                    self.doStockAction(stock, 'BUY');
                }
            };
            
            this.onStockPriceUpdated = function (symbol, price) {
                var allStocks = self.stocks();
                for (var i = 0, j = allStocks.length; i < j; i++) {
                    var currentStock = allStocks[i];
                    if (currentStock.symbol() == symbol) {
                        var currentPrice = currentStock.price();
                        if (currentPrice > price) {
                            currentStock.movementCssClass('icon-circle-arrow-down icon-white');
                        } else if (currentPrice < price) {
                            currentStock.movementCssClass('icon-circle-arrow-up icon-white');
                        }

                        currentStock.price(price);
                        currentStock.previousPrice(currentPrice);

                        var currentClassName = currentStock.cssClass();
                        var newClassName = toggleCssClass(currentClassName, 'animated', true);
                        if (newClassName != currentClassName) {
                            currentStock.cssClass(newClassName);

                            if (currentStock.timeoutID) {
                                window.clearTimeout(currentStock.timeoutID);
                                currentStock.timeoutID = null;
                            }

                            var stockToModify = currentStock;
                            currentStock.timeoutID = window.setTimeout(function () {
                                removeAnimatedCssClassCallback(stockToModify);
                            }, 2500);
                        }
                    }
                }
            };

            this.doStockAction = function(stock, action) {
                stocksHub.server.requestStockAction(Math.uuidCompact(), action, stock.symbol(), stock.quantity());
            };
            
            function unsubscribe(stock) {
                self.stocks.remove(stock);
                currentCssClassIndex--;

                var symbol = stock.symbol();

                var allStocks = self.stocks();
                for (var i = 0, j = allStocks.length; i < j; i++) {
                    var s = allStocks[i];
                    if (s.symbol() == symbol) {
                        return;
                    }
                }

                stocksHub.server.UnsubscribeFromStock(symbol);
            }

            function toggleCssClass(currentClassName, toggleClassNames, shouldHaveClass) {
                if (toggleClassNames) {
                    var cssClassNameRegex = /[\w-]+/g;
                    var currentClassNames = currentClassName.match(cssClassNameRegex) || [];

                    ko.utils.arrayForEach(toggleClassNames.match(cssClassNameRegex), function (className) {
                        var indexOfClass = ko.utils.arrayIndexOf(currentClassNames, className);
                        if (indexOfClass >= 0) {
                            if (!shouldHaveClass) {
                                currentClassNames.splice(indexOfClass, 1);
                            }
                        } else {
                            if (shouldHaveClass) {
                                currentClassNames.push(className);
                            }
                        }
                    });

                    return currentClassNames.join(' ');
                }

                throw 'toggleClassNames was not specified.';
            }
            
            function removeAnimatedCssClassCallback(stock) {
                stock.timeoutID = null;

                var currentClassName = stock.cssClass();
                var newClassName = toggleCssClass(currentClassName, 'animated', false);
                stock.cssClass(newClassName);
            }
        };
    }

    return {
        init: function () {
            var model = new viewModel();

            stocksHub = $.connection.trader;

            stocksHub.client.balanceUpdated = function (accountID, balance, stocks) {
                model.balance.onBalanceUpdated(accountID, balance, stocks);
            };

            stocksHub.client.stockActionExecuted = function (requestID, status, action, symbol, quantity, price, amount) {
                model.history.onStockActionExecuted(requestID, status, action, symbol, quantity, price, amount);
            };

            stocksHub.client.stockPriceUpdated = function (symbol, price) {
                model.trader.onStockPriceUpdated(symbol, price);
            };

            ko.applyBindings(model);
            model.startUp.start();
        }
    };
})();