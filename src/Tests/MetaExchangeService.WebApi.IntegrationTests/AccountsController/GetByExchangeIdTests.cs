using Alba;
using FluentAssertions;
using System.Net;

namespace MetaExchangeService.WebApi.IntegrationTests.AccountsController;

[TestFixture]
public class GetByExchangeIdTests : IntegrationTestBase
{
    [Test]
    public async Task GIVEN_ExistingExchangeId_WHEN_GetByExchangeIdRequested_THEN_ReturnsAccountObject()
    {
        // Arrange
        const long exchangeId = 1;
        const string expectedResult = """{"id":1,"exchange":{"id":1,"name":"Binance"},"btcAmount":2,"eurAmount":4000}""";

        await using var host = await CreateAlbaHost();

        // Act
        var result = await host.Scenario(config =>
        {
            config.Get.Url($"/exchanges/{exchangeId}/accounts");
            config.StatusCodeShouldBe(HttpStatusCode.OK);
            config.ContentTypeShouldBe("application/json; charset=utf-8");
        });

        // Assert
        result.Should().NotBeNull();
        var content = await result.ReadAsTextAsync();
        content.Should().Be(expectedResult);
    }

    [Test]
    public async Task GIVEN_WrongExchangeId_WHEN_GetByExchangeIdRequested_THEN_ReturnsProblemDetailsObject()
    {
        // Arrange
        const long exchangeId = 99;
        const string expectedResult = """
            {"type":"https://tools.ietf.org/html/rfc7231#section-6.5.4","title":"Cannot find ExchangeEntity with the id = 99","status":404,"traceId":
            """;

        await using var host = await CreateAlbaHost();

        // Act
        var result = await host.Scenario(config =>
        {
            config.Get.Url($"/exchanges/{exchangeId}/accounts");
            config.StatusCodeShouldBe(HttpStatusCode.NotFound);
            config.ContentTypeShouldBe("application/problem+json; charset=utf-8");
        });

        // Assert
        result.Should().NotBeNull();
        var content = await result.ReadAsTextAsync();
        content.Should().Contain(expectedResult);
    }
}