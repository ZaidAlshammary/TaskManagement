namespace TaskManagement.Exceptions;

public class NotAdminException : Exception
{
    public NotAdminException() : base("You are not an admin")
    {
    }
}