using System;

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
}