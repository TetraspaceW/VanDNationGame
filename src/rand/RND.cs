using System;

class RND
{
    private static readonly Random _random = new Random();
    public static int d(int n, int N = 1)
    {
        var c = 0;
        for (int i = 0; i < N; i++)
        {
            c += _random.Next(1, n + 1);
        }
        return c;
    }
}