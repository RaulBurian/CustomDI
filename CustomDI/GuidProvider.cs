namespace CustomDI;

public interface IGuidProvider
{
    Guid NewGuid();
}

public class GuidProvider : IGuidProvider
{
    private readonly Guid _guid = Guid.NewGuid();

    public Guid NewGuid() => _guid;
}
