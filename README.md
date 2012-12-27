SignalR demos
=============

This repository showcases features of [SignalR](http://www.signlar.net).  These demos were developed for a [Headspring](http://www.headspring.com) brown bag.

There are two projects in this repository

- StockTrader - a demo that shows how
 - to create, assign connections to and broadcast messages to ad-hoc groups.
 - multiple front-ends can use the same SignalR hub, with a web front-end in StockTrader.Web and a WinRT front-end in StockTrader.Windows

- VirusReplication - a demo that shows how
 - to stream data to a web front-end
 - web front-ends are not able to handle the amount of data that SignalR is capable of sending, so I present a solution for throttling virus generation calculations and streaming those results to the client.