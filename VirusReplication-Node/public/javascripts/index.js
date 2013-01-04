/// <reference path="jquery-1.8.3.min.js" />
/// <reference path="jquery.signalR-1.0.0-alpha2.min.js" />
/// <reference path="bootstrap.min.js" />
/// <reference path="knockout-2.2.0.debug.js" />
/// <reference path="easeljs-0.5.0.min.js" />

var App = (function () {
    'use strict';

    var stateColors = ['rgb(158, 11, 15)', 'rgb(160, 65, 13)', 'rgb(163, 98, 10)', 'rgb(171, 160, 0)', 'rgb(89, 133, 39)', 'rgb(25, 123, 48)', 'rgb(0, 114, 54)', 'rgb(0, 116, 107)', 'rgb(0, 74, 128)', 'rgb(0, 52, 113)'];

    function enable($element) {
        $element.removeClass('disabled').removeAttr('disabled');
    }

    function disable($element) {
        $element.addClass('disabled').attr('disabled', 'disabled');
    }

    function viewModel() {
        var self = this;

        var cellSize = 6;

        var virusHub = null;

        var canvas = null;
        var stage = null;
        var rectangleField = null;

        this.fps = ko.observable(32);
        this.q = ko.observable(10);
        this.n = ko.observable(750);
        this.k2 = ko.observable(300);

        this.start = function () {
            disable($('#start'));

            var dimensions = setupCanvas();
            var gridSize = computeGridSize(dimensions);

            connectToHub(function () {
                virusHub.server.start(self.fps(), self.q(), gridSize.x, gridSize.y, self.n(), self.k2());
            });
        };

        function setupCanvas() {
            canvas = document.getElementById('virus-canvas');

            // Set the canvas to the full window size
            var width = $(window).width();
            var height = $(window).height();

            canvas.style.width = width + 'px';
            canvas.style.height = height + 'px';
            canvas.width = canvas.offsetWidth;
            canvas.height = canvas.offsetHeight;

            // create a new stage and point it at our canvas
            stage = new createjs.Stage(canvas);

            // create a Shape instance to draw the rectangles in, and add it to the stage
            rectangleField = new createjs.Shape();
            stage.addChild(rectangleField);

            // set up the cache for the rectangle field shape, and make it the same size as the canvas
            rectangleField.cache(0, 0, canvas.width, canvas.height);

            return { width: width, height: height };
        }

        function computeGridSize(dimensions) {
            var x = Math.floor(dimensions.width / cellSize);
            var y = Math.floor(dimensions.height / cellSize);

            return { x: x, y: y };
        }

        function connectToHub(callback) {
            if (!virusHub) {
                $.connection.hub.url = 'http://localhost:10533/signalr';

                virusHub = $.connection.virus;
                virusHub.client.updateGrid = onUpdateGrid;
                virusHub.client.notifyDone = onServerDone;

                var modalShown = false;

                var timeout = window.setTimeout(function () {
                    $('#connecting-modal').modal('show');
                    modalShown = true;
                }, 1000);

                $.connection.hub.start({ transport: 'longPolling' })
                    .done(function () {
                        window.clearTimeout(timeout);

                        if (modalShown) {
                            $('#connecting-modal').modal('hide');
                        }

                        callback();
                    })
                    .fail(function () {
                        window.clearTimeout(timeout);
                        window.alert('Could not connect to hub!');
                    });
            } else {
                callback();
            }
        }

        function onUpdateGrid(cells) {
            for (var i = 0; i < cells.length; i++) {
                var cell = cells[i];

                var stateColor = stateColors[(cell.state - 1) % 10];

                rectangleField.graphics.beginFill(stateColor).rect(cell.x * (cellSize + 1), cell.y * (cellSize + 1), cellSize, cellSize);
            }

            rectangleField.updateCache('source-overlay');
            rectangleField.graphics.clear();

            stage.update();
        }

        function onServerDone() {
            enable($('#start'));
        }
    }

    return {
        init: function () {
            var model = new viewModel();
            ko.applyBindings(model);

            enable($('#start'));
        }
    };
})();
