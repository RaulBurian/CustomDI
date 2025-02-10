namespace CustomDI;

public class RandomNumberService
{
    private readonly int _number = Random.Shared.Next();

    public int GetRandom() => _number;
}
