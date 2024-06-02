using TaskManagement.Models;

namespace TaskManagement.Dtos.Tasks;

public class TaskCreateDto
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime DueDate { get; set; }
    public Status Status { get; set; }
    public string AssignedTo { get; set; } = null!;
}