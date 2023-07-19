using MetaExchangeService.Application.Services;
using MetaExchangeService.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace MetaExchangeService.WebApi.Controllers;

/// <summary>
/// Exchange accounts management.
/// </summary>
[ApiController]
[Route("[controller]")]
public sealed class AccountsController : ControllerBase
{
    private readonly IAccountsService _accountsService;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="accountsService"></param>
    public AccountsController(IAccountsService accountsService) =>
        _accountsService = accountsService;

    /// <summary>
    /// Get exchange account information.
    /// </summary>
    /// <param name="exchangeId">Exchange identifier.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("/exchanges/{exchange-id}/[controller]")]
    [ProducesResponseType(typeof(Account), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public Task<Account?> GetByExchangeId(
        [FromRoute(Name = "exchange-id")] long exchangeId,
        CancellationToken cancellationToken) =>
        _accountsService.GetByExchangeAsync(exchangeId, cancellationToken);

    /// <summary>
    /// Get all exchange accounts information.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(Account), StatusCodes.Status200OK)]
    public Task<IEnumerable<Account>> GetAll(CancellationToken cancellationToken) =>
        _accountsService.GetAllAsync(cancellationToken);
}