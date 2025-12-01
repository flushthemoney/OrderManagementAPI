using System.ComponentModel.DataAnnotations;

namespace OrderManagementAPI.DTOs;

public class CreateOrderDto
{
    [Required]
    public string ProductName { get; set; } = string.Empty;
    
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
    public int Quantity { get; set; }
    
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Unit price must be greater than 0")]
    public decimal UnitPrice { get; set; }
}

public class UpdateOrderDto
{
    public string? ProductName { get; set; }
    
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
    public int? Quantity { get; set; }
    
    [Range(0.01, double.MaxValue, ErrorMessage = "Unit price must be greater than 0")]
    public decimal? UnitPrice { get; set; }
}

public class OrderDto
{
    public int Id { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; }
}
