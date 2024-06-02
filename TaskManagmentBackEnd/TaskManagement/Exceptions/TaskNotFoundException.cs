namespace TaskManagement.Exceptions;

public class TaskNotFoundException : Exception
{
    public TaskNotFoundException(int taskId) : base($"Task with id {taskId} not found")
    {
    }
}