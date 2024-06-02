using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Models;

public class Entity
{
    [Key]
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
}