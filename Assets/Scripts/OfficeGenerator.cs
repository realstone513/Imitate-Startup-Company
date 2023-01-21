using UnityEngine;

public class OfficeGenerator : MonoBehaviour
{
    public GameObject wallStandard;
    public GameObject wallT;
    public GameObject wallCross;
    public GameObject wallCorner;
    public GameObject wallDoor;

    public void GenerateWall(Vector3 startPos, int row, int[] data)
    {
        Vector3 interval = Vector3.zero;
        Quaternion qi = Quaternion.identity;
        int col = data.Length / row;

        for (int c = 0; c < col; c++)
        {
            for (int r = 0; r < row; r++)
            {
                if (data[c * row + r] == 1)
                    Instantiate(wallStandard, startPos + interval, qi);
                interval.x += 1;
            }
            interval.x = startPos.x;
            interval.z -= 1;
        }
    }

    private void Start()
    {
        int row = 13;
        int[] datas =
        {
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 1,
            1, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 1,
            1, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 1,
            1, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1,
            1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1,
            1, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1,
            1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
        };

        GenerateWall(Vector3.zero, row, datas);
    }
}