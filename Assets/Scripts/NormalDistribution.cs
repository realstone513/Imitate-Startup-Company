using UnityEngine;

public class NormalDistribution
{
    public static float GetData(float min, float max)
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

    public static int[] GetRange(int tryCount, int min, int max)
    {
        int size = max - min + 1;
        int[] result = new int[size];

        for (int i = 0; i < tryCount; i++)
        {
            int num = (int)(GetData(min, max) + 0.5f);
            result[num - min]++;
        }
        return result;
    }
}