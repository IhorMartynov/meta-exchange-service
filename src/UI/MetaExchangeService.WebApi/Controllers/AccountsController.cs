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
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
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

    /// <summary>
    /// Create new account.
    /// </summary>
    /// <param name="account">Account details.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(Account), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAccount([FromBody] CreateAccountModel account,
        CancellationToken cancellationToken)
    {
        var entity = await _accountsService.CreateAsync(account, cancellationToken);

        return CreatedAtAction(nameof(GetByExchangeId),
            new Dictionary<string, object> {{"exchange-id", entity.Exchange!.Id}},
            entity);
    }

    /// <summary>
    /// Update exchange account information.
    /// </summary>
    /// <param name="exchangeId">Exchange identifier.</param>
    /// <param name="account">Account details.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPatch("/exchanges/{exchange-id}/[controller]")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public Task UpdateAccount(
        [FromRoute(Name = "exchange-id")] long exchangeId,
        [FromBody] UpdateAccountModel account,
        CancellationToken cancellationToken) =>
        _accountsService.UpdateAccountAsync(exchangeId, account, cancellationToken);

    /// <summary>
    /// Update exchange account information.
    /// </summary>
    /// <param name="exchangeId">Exchange identifier.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete("/exchanges/{exchange-id}/[controller]")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public Task DeleteAccount(
        [FromRoute(Name = "exchange-id")] long exchangeId,
        CancellationToken cancellationToken) =>
        _accountsService.DeleteAccountAsync(exchangeId, cancellationToken);
}