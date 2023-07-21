# Meta-Exchange service.

Test task created for the Boerse Stuttgart Digital.

## Requirements

### Part 1

The task is to implement a meta-exchange that always gives the user the best possible price if he is buying or selling a certain amount of BTC.
Your algorithm needs to output one or more buy or sell orders that our system called Hedger issues to one or more of these n crypto-exchanges. In effect, our user buys the specified amount of BTC for the lowest possible price or sells the specified amount of BTC for the highest possible price.
To make life a bit more complicated, each cryptocurrency comes with info about how much money (EUR) and how much BTC we have there on our account (balance). Your algorithm needs to achieve the best price within these constraints. The algorithm cannot transfer any money and crypto between crypto exchanges.
Your solution should be a relatively simple .NET Core console-mode application, which reads the order books, balance constraints, and order type/size, and outputs (to console) a set of orders to execute against the given order books (exchanges). Please prepare a function that solves the task.

### Part 2

Implement a Web service (Kestrel, .NET Core API), and expose the implemented functionality through it. Implement an endpoint that will receive the required parameters (the type of order and the amount of BTC that our user wants to buy or sell), and return a JSON response with the "best execution" plan.

### BONUS TASKS

- Write some tests, on relatively simple input data (e.g., order books with only a few bids and asks), to test your solution on typical and edge cases.
- Deploy your Web service locally with Docker.

## Solution

The solution follows the Clean Architecture pattern and consists of 4 layers:
 - Domain
	 - MetaExchangeService.Domain
 - Infrastructure
	 - MetaExchangeService.Repositories
	 - MetaExchangeService.Repositories.Contracts
 - Application
	 - MetaExchangeService.Application
 - UI
	 - MetaExchangeService.Console
	 - MetaExchangeService.WebApi

It uses an MS SQL Server to store order books and exchange accounts. The database contains 3 tables:
 - Exchanges
 - Accounts
 - Orders

## Running solution

### Web API project

The solution contains the docker-compose.yml file to help to run it locally. To start the project you can use Visual Studio or run
`
docker compose up
`
command from the src folder. And then navigate to the [local swagger page](https://localhost:52708/swagger/index.html).

### Console application project

To run the console application use the following command:
`
MetaExchangeService.Console.exe btc-amount=<amount> operation-type=<buy or sell>
`
