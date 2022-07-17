using System;
using System.Linq;
class RND
{
    private static readonly Random _random = new Random();
    public static int d(int n, int N = 1)
    {
        return Enumerable.Range(0, N)
        .Select(_ => { return _random.Next(1, n + 1); })
        .Sum();
    }

    public static int Next() { return _random.Next(); }
    public static int Next(int minValue, int maxValue) { return _random.Next(minValue, maxValue); }
    public static double NextDouble() { return _random.NextDouble(); }
}