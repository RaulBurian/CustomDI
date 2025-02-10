namespace CustomDI.DependencyInjection;

public class NotRegisteredException: Exception
{
    public NotRegisteredException()
    {
    }

    public NotRegisteredException(string? message) : base(message)
    {
    }

    public NotRegisteredException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
