using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagementAPI.Data;
using OrderManagementAPI.DTOs;
using OrderManagementAPI.Models;
using System.Security.Claims;

namespace OrderManagementAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public OrdersController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Orders
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var orders = await _context.Orders
            .Where(o => o.UserId == userId)
            .Select(o => new OrderDto
            {
                Id = o.Id,
                ProductName = o.ProductName,
                Quantity = o.Quantity,
                UnitPrice = o.UnitPrice,
                TotalAmount = o.TotalAmount,
                CreatedAt = o.CreatedAt
            })
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();

        return Ok(orders);
    }

    // GET: api/Orders/5
    [HttpGet("{id}")]
    public async Task<ActionResult<OrderDto>> GetOrder(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var order = await _context.Orders
            .Where(o => o.Id == id && o.UserId == userId)
            .FirstOrDefaultAsync();

        if (order == null)
            return NotFound(new { message = "Order not found" });

        var orderDto = new OrderDto
        {
            Id = order.Id,
            ProductName = order.ProductName,
            Quantity = order.Quantity,
            UnitPrice = order.UnitPrice,
            TotalAmount = order.TotalAmount,
            CreatedAt = order.CreatedAt
        };

        return Ok(orderDto);
    }

    // POST: api/Orders
    [HttpPost]
    public async Task<ActionResult<OrderDto>> CreateOrder([FromBody] CreateOrderDto orderDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var order = new Order
        {
            ProductName = orderDto.ProductName,
            Quantity = orderDto.Quantity,
            UnitPrice = orderDto.UnitPrice,
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        var createdOrderDto = new OrderDto
        {
            Id = order.Id,
            ProductName = order.ProductName,
            Quantity = order.Quantity,
            UnitPrice = order.UnitPrice,
            TotalAmount = order.TotalAmount,
            CreatedAt = order.CreatedAt
        };

        return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, createdOrderDto);
    }

    // PUT: api/Orders/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrder(int id, [FromBody] UpdateOrderDto orderDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var order = await _context.Orders
            .Where(o => o.Id == id && o.UserId == userId)
            .FirstOrDefaultAsync();

        if (order == null)
            return NotFound(new { message = "Order not found" });

        // Update only provided fields
        if (!string.IsNullOrWhiteSpace(orderDto.ProductName))
            order.ProductName = orderDto.ProductName;

        if (orderDto.Quantity.HasValue)
            order.Quantity = orderDto.Quantity.Value;

        if (orderDto.UnitPrice.HasValue)
            order.UnitPrice = orderDto.UnitPrice.Value;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!OrderExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // DELETE: api/Orders/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var order = await _context.Orders
            .Where(o => o.Id == id && o.UserId == userId)
            .FirstOrDefaultAsync();

        if (order == null)
            return NotFound(new { message = "Order not found" });

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool OrderExists(int id)
    {
        return _context.Orders.Any(e => e.Id == id);
    }
}
