namespace AdventOfCode.Year2023;

public class ItWontHappenException : Exception
{
    public ItWontHappenException() { }
    public ItWontHappenException(string message) : base(message) { }
}
