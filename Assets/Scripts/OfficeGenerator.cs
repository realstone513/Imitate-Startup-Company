using System.Collections.Generic;
using UnityEngine;
using System.IO;

public enum Direction
{
    Up, Down, Left, Right
}

public class OfficeGenerator : MonoBehaviour
{
    public GameObject wallStandard;
    public GameObject wallT;
    public GameObject wallCross;
    public GameObject wallCorner;
    public GameObject wallDoor;

    private List<GameObject> walls = new();
    private List<int> data = new();
    private int row;
    private int col;

    public void GenerateWall(Vector3 startPos)
    {
        Quaternion qi = Quaternion.identity;
        row = data.Count / col;
        startPos.x -= col / 2;
        startPos.z += row / 2;
        Vector3 interval = Vector3.zero;

        for (int r = 0; r < row; r++)
        {
            for (int c  = 0; c < col; c++)
            {
                if (data[r * col + c] == 1)
                {
                    GameObject wall;
                    bool[] flags = {
                        CheckEdge(r - 1, c),
                        CheckEdge(r + 1, c),
                        CheckEdge(r, c - 1),
                        CheckEdge(r, c + 1)
                    };

                    int count = 0;
                    for (int i = 0; i < 4; i++)
                        if (flags[i]) count++;

                    if (count == 4)
                        wall = Instantiate(wallCross, startPos + interval, qi, gameObject.transform);
                    else if (count == 3)
                    {
                        wall = Instantiate(wallT, startPos + interval, qi, gameObject.transform);
                        if (!flags[(int)Direction.Left])
                            wall.transform.Rotate(new Vector3(0, 90, 0));
                        else if (!flags[(int)Direction.Up])
                            wall.transform.Rotate(new Vector3(0, 180, 0));
                        else if (!flags[(int)Direction.Right])
                            wall.transform.Rotate(new Vector3(0, 270, 0));
                    }
                    else
                    {
                        if (flags[(int)Direction.Up] && flags[(int)Direction.Down])
                        {
                            wall = Instantiate(wallStandard, startPos + interval, qi, gameObject.transform);
                            if (flags[(int)Direction.Up] && flags[(int)Direction.Down])
                                wall.transform.Rotate(new Vector3(0, 90, 0));
                        }
                        else if (flags[(int)Direction.Left] && flags[(int)Direction.Right])
                            wall = Instantiate(wallStandard, startPos + interval, qi, gameObject.transform);
                        else
                        {
                            wall = Instantiate (wallCorner, startPos + interval, qi, gameObject.transform);

                            if (flags[(int)Direction.Up] && flags[(int)Direction.Right])
                                wall.transform.Rotate(new Vector3(0, 90, 0));
                            else if (flags[(int)Direction.Down] && flags[(int)Direction.Right])
                                wall.transform.Rotate(new Vector3(0, 180, 0));
                            else if (flags[(int)Direction.Down] && flags[(int)Direction.Left])
                                wall.transform.Rotate(new Vector3(0, 270, 0));
                        }
                    }

                    walls.Add(wall);
                }
                interval.x += 1;
            }
            interval.x = 0;
            interval.z -= 1;
        }
    }

    private bool CheckEdge(int r, int c)
    {
        // out range
        if (r < 0 || r > row - 1 || c < 0 || c > col - 1)
            return false;
        if (data[r * col + c] == 0)
            return false;
        return true;
    }

    private void ReadData()
    {
        StreamReader sr = new("Assets/Resources/Map.txt");

        string line = sr.ReadLine();
        col = int.Parse(line);

        while ((line = sr.ReadLine()) != null)
        {
            foreach (char c in line)
            {
                data.Add(c - '0');
            }
        }
        sr.Close();
    }

    private void Start()
    {
        ReadData();
        GenerateWall(Vector3.zero);
    }
}