namespace TaskManagement.Dtos.Auth;

public class CreateUserDto
{
    public string Name { get; set; } = null!;
    public string Password { get; set; } = null!;
}