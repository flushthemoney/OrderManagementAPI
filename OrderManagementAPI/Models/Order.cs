using System.ComponentModel.DataAnnotations;

namespace OrderManagementAPI.Models;

public class Order
{
    public int Id { get; set; }
    
    [Required]
    public string ProductName { get; set; } = string.Empty;
    
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
    public int Quantity { get; set; }
    
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Unit price must be greater than 0")]
    public decimal UnitPrice { get; set; }
    
    public decimal TotalAmount => Quantity * UnitPrice;
    
    // Foreign key to AspNetUsers
    [Required]
    public string UserId { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
