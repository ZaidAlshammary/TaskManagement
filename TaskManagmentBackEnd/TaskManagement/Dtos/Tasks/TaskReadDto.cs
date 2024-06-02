using TaskManagement.Models;

namespace TaskManagement.Dtos.Tasks;

public class TaskReadDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime DueDate { get; set; }
    public Status Status { get; set; }
    public string Assignee { get; set; } = null!;
    public string AssigneeId { get; set; } = null!;
}