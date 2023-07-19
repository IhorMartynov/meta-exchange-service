using System.ComponentModel.DataAnnotations;
using MetaExchangeService.Application.Services;
using MetaExchangeService.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace MetaExchangeService.WebApi.Controllers;

/// <summary>
/// Orders management.
/// </summary>
[ApiController]
[Route("[controller]")]
public sealed class OrdersController : ControllerBase
{
    private readonly IOrdersService _ordersService;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="ordersService"></param>
    public OrdersController(IOrdersService ordersService)
    {
        _ordersService = ordersService;
    }

    /// <summary>
    /// Get order.
    /// </summary>
    /// <param name="id">Order identifier.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
    public Task<Order?> GetById([FromRoute] long id, CancellationToken cancellationToken) =>
        _ordersService.GetAsync(id, cancellationToken);

    /// <summary>
    /// Get all orders.
    /// </summary>
    /// <param name="page">Page number (starting from 1).</param>
    /// <param name="pageSize">Page size.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Order>), StatusCodes.Status200OK)]
    public Task<IEnumerable<Order>> GetAll([FromQuery, Required] int page = 1,
        [FromQuery, Required] int pageSize = 10,
        CancellationToken cancellationToken = default) =>
        _ordersService.GetAllAsync(page, pageSize, cancellationToken);

    /// <summary>
    /// Create an order.
    /// </summary>
    /// <param name="exchangeId">Exchange identifier.</param>
    /// <param name="order">Order details.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("/exchanges/{exchange-id}/[controller]")]
    [ProducesResponseType(typeof(Order), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateOrder(
        [FromRoute(Name = "exchange-id")] long exchangeId,
        [FromBody] CreateOrderModel order,
        CancellationToken cancellationToken)
    {
        var entity = await _ordersService.CreateAsync(exchangeId, order, cancellationToken);

        return CreatedAtAction(nameof(GetById), new {id = entity.Id}, entity);
    }

    /// <summary>
    /// Create orders.
    /// </summary>
    /// <param name="exchangeId">Exchange identifier.</param>
    /// <param name="orders">Array of order details.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("/exchanges/{exchange-id}/[controller]/bulk")]
    [ProducesResponseType(typeof(IEnumerable<Order>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateOrders(
        [FromRoute(Name = "exchange-id")] long exchangeId,
        [FromBody] CreateOrderModel[] orders,
        CancellationToken cancellationToken)
    {
        var entities = await _ordersService.CreateAsync(exchangeId, orders, cancellationToken);

        return CreatedAtAction(nameof(GetAll), new { page = 0, pageSize = 10 }, entities);
    }

    /// <summary>
    /// Delete an order.
    /// </summary>
    /// <param name="id">Order identifier.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public Task DeleteOrder([FromRoute] long id, CancellationToken cancellationToken) =>
        _ordersService.DeleteAsync(id, cancellationToken);
}