using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Models;

public class Task : Entity
{
    private const int GUID_LENGTH = 36;
    
    [MaxLength(20)]
    public string Title { get; set; } = null!;

    [MaxLength(200)]
    public string? Description { get; set; }
    
    public DateTime DueDate { get; set; }
    
    public Status Status { get; set; } = Status.Pending;
    public User User { get; set; } = null!;
    
    [MaxLength(GUID_LENGTH)]
    public string UserId { get; set; } = null!;
}

public enum Status
{
    Pending = 0,
    Complete = 1,
}