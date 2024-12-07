namespace AdventOfCode.Year2024;

public class ItWontHappenException : Exception
{
    public ItWontHappenException() { }
    public ItWontHappenException(string message) : base(message) { }
}
