using System;
using System.Linq;

public static class Helpers
{
    static int gcd(int n1, int n2)
    {
        if (n2 == 0)
        {
            return n1;
        }
        else
        {
            return gcd(n2, n1 % n2);
        }
    }

    public static int LeastCommonMultiple(int[] numbers)
    {
        return numbers.Aggregate((S, val) => S * val / gcd(S, val));
    }
}
