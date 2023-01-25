using UnityEngine;

public class NormalDistribution
{
    public static float RangeAdditive(float min, float max)
    {
        float sum = 0;
        int i;
        int count = 2;
        (float x, float y)[] values = new (float x, float y)[count];

        for (i = 0; i < values.Length; i++)
            values[i] = (min / count, max / count);

        for (i = 0; i < values.Length; i++)
            sum += Random.Range(values[i].x, values[i].y);

        return sum;
    }
}