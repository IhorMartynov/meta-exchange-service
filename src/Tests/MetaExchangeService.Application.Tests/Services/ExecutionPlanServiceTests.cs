using MetaExchangeService.Application.Services;
using MetaExchangeService.Domain.Models;
using MetaExchangeService.Repositories.Contracts.Repositories;

namespace MetaExchangeService.Application.Tests.Services;

[TestFixture]
public sealed class ExecutionPlanServiceTests
{
    #region GetBestBuyingPlanAsync Tests

    [Test]
    public async Task GIVEN_BtcAmountIsZero_WHEN_GetBestBuyingPlanAsync_THEN_ReturnsEmptyCollection()
    {
        // Arrange
        var accountsRepositoryMock = new Mock<IAccountsRepository>();
        var ordersRepositoryMock = new Mock<IOrdersRepository>();

        var service = new ExecutionPlanService(accountsRepositoryMock.Object, ordersRepositoryMock.Object);

        // Act
        var result = await service.GetBestBuyingPlanAsync(0, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Test]
    public async Task GIVEN_AccountEurAmountIsZero_WHEN_GetBestBuyingPlanAsync_THEN_ReturnsEmptyCollection()
    {
        // Arrange
        var accountsRepositoryMock = new Mock<IAccountsRepository>();
        var ordersRepositoryMock = new Mock<IOrdersRepository>();

        accountsRepositoryMock.Setup(repository => repository.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new[] {new Account {Id = 5, Exchange = new Exchange {Id = 1}, BtcAmount = 3, EurAmount = 0}});

        var service = new ExecutionPlanService(accountsRepositoryMock.Object, ordersRepositoryMock.Object);

        // Act
        var result = await service.GetBestBuyingPlanAsync(2, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Test]
    public async Task GIVEN_ExchangeHasNoAskOrders_WHEN_GetBestBuyingPlanAsync_THEN_ReturnsEmptyArray()
    {
        // Arrange
        var accountsRepositoryMock = new Mock<IAccountsRepository>();
        var ordersRepositoryMock = new Mock<IOrdersRepository>();

        accountsRepositoryMock.Setup(repository => repository.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new[] { new Account { Id = 5, Exchange = new Exchange { Id = 1 }, BtcAmount = 3, EurAmount = 2000 } })
            .Verifiable();

        ordersRepositoryMock.Setup(repository => repository.GetAskOrdersSortedByPriceAscendingAsync(
                It.Is<int>(x => x == 1), It.Is<int>(x => x == 10), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Order>())
            .Verifiable();

        var service = new ExecutionPlanService(accountsRepositoryMock.Object, ordersRepositoryMock.Object);

        // Act
        var result = await service.GetBestBuyingPlanAsync(2, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
        accountsRepositoryMock.Verify();
        ordersRepositoryMock.Verify();
    }

    [Test]
    public async Task GIVEN_BtcAmount_WHEN_GetBestBuyingPlanAsync_THEN_ReturnsOrdersWithLowestPrice()
    {
        // Arrange
        var accountsRepositoryMock = new Mock<IAccountsRepository>();
        var ordersRepositoryMock = new Mock<IOrdersRepository>();

        var accounts = new[]
        {
            new Account {Id = 5, Exchange = new Exchange {Id = 1}, BtcAmount = 3, EurAmount = 2000},
            new Account {Id = 6, Exchange = new Exchange {Id = 2}, BtcAmount = 5, EurAmount = 4006}
        };

        var ordersPage1 = new[]
        {
            new Order {Id = 1, Exchange = new Exchange {Id = 1}, Type = OrderType.Ask, Amount = 2, Price = 2000},
            new Order {Id = 2, Exchange = new Exchange {Id = 1}, Type = OrderType.Ask, Amount = 1, Price = 2000}
        };
        var ordersPage2 = new[]
        {
            new Order {Id = 3, Exchange = new Exchange {Id = 2}, Type = OrderType.Ask, Amount = 1, Price = 2001},
            new Order {Id = 4, Exchange = new Exchange {Id = 2}, Type = OrderType.Ask, Amount = 2, Price = 2005},
            new Order {Id = 5, Exchange = new Exchange {Id = 2}, Type = OrderType.Ask, Amount = 2, Price = 2006}
        };

        accountsRepositoryMock.Setup(repository => repository.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(accounts)
            .Verifiable();

        ordersRepositoryMock.Setup(repository => repository.GetAskOrdersSortedByPriceAscendingAsync(
                It.Is<int>(x => x == 1), It.Is<int>(x => x == 10), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ordersPage1)
            .Verifiable();
        ordersRepositoryMock.Setup(repository => repository.GetAskOrdersSortedByPriceAscendingAsync(
                It.Is<int>(x => x == 2), It.Is<int>(x => x == 10), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ordersPage2)
            .Verifiable();
        ordersRepositoryMock.Setup(repository => repository.GetAskOrdersSortedByPriceAscendingAsync(
                It.Is<int>(x => x > 2), It.Is<int>(x => x == 10), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Enumerable.Empty<Order>());

        var service = new ExecutionPlanService(accountsRepositoryMock.Object, ordersRepositoryMock.Object);

        // Act
        var result = (await service.GetBestBuyingPlanAsync(3, CancellationToken.None))
            .ToArray();

        // Assert
        result.Should().HaveCount(3);
        result[0].Should().Match<ExecutionPlanItem>(x => x.Order.Id == 1 && x.BtcAmount == 1);
        result[1].Should().Match<ExecutionPlanItem>(x => x.Order.Id == 3 && x.BtcAmount == 1);
        result[2].Should().Match<ExecutionPlanItem>(x => x.Order.Id == 4 && x.BtcAmount == 1);
        accountsRepositoryMock.Verify();
        ordersRepositoryMock.Verify();
    }

    #endregion

    #region GetBestSellingPlanAsync Tests

    [Test]
    public async Task GIVEN_BtcAmountIsZero_WHEN_GetBestSellingPlanAsync_THEN_ReturnsEmptyCollection()
    {
        // Arrange
        var accountsRepositoryMock = new Mock<IAccountsRepository>();
        var ordersRepositoryMock = new Mock<IOrdersRepository>();

        var service = new ExecutionPlanService(accountsRepositoryMock.Object, ordersRepositoryMock.Object);

        // Act
        var result = await service.GetBestSellingPlanAsync(0, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Test]
    public async Task GIVEN_AccountBtcAmountIsZero_WHEN_GetBestSellingPlanAsync_THEN_ReturnsEmptyCollection()
    {
        // Arrange
        var accountsRepositoryMock = new Mock<IAccountsRepository>();
        var ordersRepositoryMock = new Mock<IOrdersRepository>();

        accountsRepositoryMock.Setup(repository => repository.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new[] { new Account { Id = 5, Exchange = new Exchange { Id = 1 }, BtcAmount = 0, EurAmount = 3000 } });

        var service = new ExecutionPlanService(accountsRepositoryMock.Object, ordersRepositoryMock.Object);

        // Act
        var result = await service.GetBestSellingPlanAsync(2, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Test]
    public async Task GIVEN_ExchangeHasNoBidOrders_WHEN_GetBestSellingPlanAsync_THEN_ReturnsEmptyArray()
    {
        // Arrange
        var accountsRepositoryMock = new Mock<IAccountsRepository>();
        var ordersRepositoryMock = new Mock<IOrdersRepository>();

        accountsRepositoryMock.Setup(repository => repository.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new[] { new Account { Id = 5, Exchange = new Exchange { Id = 1 }, BtcAmount = 3, EurAmount = 2000 } })
            .Verifiable();

        ordersRepositoryMock.Setup(repository => repository.GetBidOrdersSortedByPriceDescendingAsync(
                It.Is<int>(x => x == 1), It.Is<int>(x => x == 10), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Order>())
            .Verifiable();

        var service = new ExecutionPlanService(accountsRepositoryMock.Object, ordersRepositoryMock.Object);

        // Act
        var result = await service.GetBestSellingPlanAsync(2, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
        accountsRepositoryMock.Verify();
        ordersRepositoryMock.Verify();
    }

    [Test]
    public async Task GIVEN_BtcAmount_WHEN_GetBestSellingPlanAsync_THEN_ReturnsOrdersWithHighestPrice()
    {
        // Arrange
        var accountsRepositoryMock = new Mock<IAccountsRepository>();
        var ordersRepositoryMock = new Mock<IOrdersRepository>();

        var accounts = new[]
        {
            new Account {Id = 5, Exchange = new Exchange {Id = 1}, BtcAmount = 1, EurAmount = 2000},
            new Account {Id = 6, Exchange = new Exchange {Id = 2}, BtcAmount = 5, EurAmount = 4006}
        };

        var ordersPage1 = new[]
        {
            new Order {Id = 1, Exchange = new Exchange {Id = 1}, Type = OrderType.Bid, Amount = 2, Price = 2010},
            new Order {Id = 2, Exchange = new Exchange {Id = 1}, Type = OrderType.Bid, Amount = 1, Price = 2009}
        };
        var ordersPage2 = new[]
        {
            new Order {Id = 3, Exchange = new Exchange {Id = 2}, Type = OrderType.Bid, Amount = 1, Price = 2009},
            new Order {Id = 4, Exchange = new Exchange {Id = 2}, Type = OrderType.Bid, Amount = 2, Price = 2008},
            new Order {Id = 5, Exchange = new Exchange {Id = 2}, Type = OrderType.Bid, Amount = 2, Price = 2006}
        };

        accountsRepositoryMock.Setup(repository => repository.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(accounts)
            .Verifiable();

        ordersRepositoryMock.Setup(repository => repository.GetBidOrdersSortedByPriceDescendingAsync(
                It.Is<int>(x => x == 1), It.Is<int>(x => x == 10), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ordersPage1)
            .Verifiable();
        ordersRepositoryMock.Setup(repository => repository.GetBidOrdersSortedByPriceDescendingAsync(
                It.Is<int>(x => x == 2), It.Is<int>(x => x == 10), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ordersPage2)
            .Verifiable();
        ordersRepositoryMock.Setup(repository => repository.GetBidOrdersSortedByPriceDescendingAsync(
                It.Is<int>(x => x > 2), It.Is<int>(x => x == 10), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Enumerable.Empty<Order>());

        var service = new ExecutionPlanService(accountsRepositoryMock.Object, ordersRepositoryMock.Object);

        // Act
        var result = (await service.GetBestSellingPlanAsync(3, CancellationToken.None))
            .ToArray();

        // Assert
        result.Should().HaveCount(3);
        result[0].Should().Match<ExecutionPlanItem>(x => x.Order.Id == 1 && x.BtcAmount == 1);
        result[1].Should().Match<ExecutionPlanItem>(x => x.Order.Id == 3 && x.BtcAmount == 1);
        result[2].Should().Match<ExecutionPlanItem>(x => x.Order.Id == 4 && x.BtcAmount == 1);
        accountsRepositoryMock.Verify();
        ordersRepositoryMock.Verify();
    }

    #endregion
}