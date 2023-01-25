using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum Gender
{
    Male,
    Female,
}

public class EmployeeGenerator : MonoBehaviour
{
    private List<Dictionary<string, string>> nameTable = new();
    private int tableLength;

    public int tryCount = 100000;
    public int variance = 5;
    public int rangeMin = 1;
    public int rangeMax = 10;
    [Range(0f, 1f)]
    public float diff = 0f;

    private void Start()
    {
        nameTable = CSVReader.Read("NameTable");
        tableLength = nameTable.Count;
        TestND();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TestND();
        }
    }

    private void TestND()
    {
        int size = rangeMax + 1;
        int[] result = new int[size];

        for (int i = 0; i < tryCount; i++)
        {
            int randomNumber =
                Mathf.RoundToInt(
                    NormalDistribution.RangeAdditive(
                    rangeMin, rangeMax, variance, diff));

            result[randomNumber]++;
        }

        for (int i = rangeMin; i < size; i++)
            Debug.Log($"{i}:{result[i]}");
    }

    private string CreateName()
    {
        Gender gender = (Gender)Random.Range(0, 2);
        Debug.Log(gender);
        int selectFirst = Random.Range(0, tableLength);
        int selectLast = Random.Range(0, tableLength);
        if (gender == Gender.Male)
        {
            Debug.Log($"{nameTable[selectFirst]["First"]} {nameTable[selectLast]["Male"]}");
        }
        else
        {
            Debug.Log($"{nameTable[selectFirst]["First"]} {nameTable[selectLast]["Female"]}");
        }
        return "";
    }
}