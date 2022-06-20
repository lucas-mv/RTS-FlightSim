using System;
using System.Linq;
using UnityEngine;

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

    public static void SetHardwareAltitudeAlert(bool alertValue)
    {
        PlayerPrefs.SetInt(Constants.PLANE_ALERT_ALTITUDE, alertValue ? 1 : 0);
    }

    public static void SetHardwareProximityAlert(bool alertValue)
    {
        PlayerPrefs.SetInt(Constants.PLANE_ALERT_PROXIMITY, alertValue ? 1 : 0);
    }
}
