namespace AoC2022.Days1to10;

public class ArgumentOutOfRangeExceptionWithCheck : ArgumentOutOfRangeException
{
    public static void ThrowIfNotRange(string[] array, int size)
    {
        if (array.Length > size)
            throw new ArgumentOutOfRangeException();
    }
    public static void ThrowIfNotValid(string value, string[] allowed)
    {
        if(!allowed.Contains(value))
            throw new ArgumentOutOfRangeException();
    }
}