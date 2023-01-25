using UnityEngine;

public class NormalDistribution
{
    public static float RangeAdditive(float min, float max, int variance)
    {
        float sum = 0;
        int i;
        int modVar = variance + 1;

        (float x, float y)[] values = new (float x, float y)[modVar];

        for (i = 0; i < values.Length; i++)
            values[i] = (min / modVar, max / modVar);

        for (i = 0; i < values.Length; i++)
            sum += Random.Range(values[i].x, values[i].y);

        return sum;
    }
}