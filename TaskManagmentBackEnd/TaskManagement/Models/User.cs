using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace TaskManagement.Models;

public class User : IdentityUser
{
    [MaxLength(20)]
    public string FullName { get; set; } = null!;
    public bool IsAdmin { get; set; } = false;

    public List<Task> Tasks { get; set; } = new();
}