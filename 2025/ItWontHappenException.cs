namespace AdventOfCode.Year2025;

public class ItWontHappenException : Exception
{
    public ItWontHappenException() { }
    public ItWontHappenException(string message) : base(message) { }
}
