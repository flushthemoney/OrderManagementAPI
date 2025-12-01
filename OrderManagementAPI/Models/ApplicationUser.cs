using Microsoft.AspNetCore.Identity;

namespace OrderManagementAPI.Models;

public class ApplicationUser : IdentityUser
{
    // You can add additional properties here if needed
    public string? FullName { get; set; }
}
