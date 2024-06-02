namespace TaskManagement.Resolvers;

public interface IUserContextResolver
{
    string? CurrentUserId { get; }
    bool IsAdmin { get; }
}

public class UserContextResolver : IUserContextResolver
{
    private readonly IHttpContextAccessor _contextAccessor;

    public UserContextResolver(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public string? CurrentUserId => _contextAccessor.HttpContext?.User?.FindFirst("Id")?.Value;
    public bool IsAdmin => _contextAccessor.HttpContext?.User?.FindFirst("IsAdmin")?.Value?.Equals("true", StringComparison.OrdinalIgnoreCase) ?? false;
}