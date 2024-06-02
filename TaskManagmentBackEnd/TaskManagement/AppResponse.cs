namespace TaskManagement;

public class AppResponse
{
    public int Code { get; set; }
    public string? Message { get; set; }
    public object? Data { get; set; }
}