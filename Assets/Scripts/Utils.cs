using UnityEngine;

public static class Utils
{
    public static float GetTupleRatio((float, float) tuple)
    {
        return tuple.Item1 / tuple.Item2;
    }

    public static float GetTupleRatio((int, int) tuple)
    {
        return (float)tuple.Item1 / (float)tuple.Item2;
    }

    public static float GetAverage(params float[] prs)
    {
        float sum = 0f;
        foreach (var f in prs)
        {
            sum += f;
        }
        return sum / prs.Length;
    }

    public static int GetNumberFromDate(Date date)
    {
        return (date.day - 1) + (date.month - 1) * 4 + (date.year - 1) * 48;
    }

    public static void CopyTransform(GameObject gameObject, Transform dest)
    {
        gameObject.transform.position = dest.position;
        gameObject.transform.rotation = dest.rotation;
        gameObject.transform.localScale = dest.localScale;
    }

    public static float GetMinuteFromTime((float hour, float minute) time)
    {
        return time.hour * 60 + time.minute;
    }

    public static float CalculateTimeGap((float hour, float minute) greater, (float hour, float minute) smaller)
    {
        return GetMinuteFromTime(greater) - GetMinuteFromTime(smaller);
    }

    public static (float hour, float minute) GetTimeFromMinute(float minute)
    {
        return (minute % 60, minute / 60);
    }
}