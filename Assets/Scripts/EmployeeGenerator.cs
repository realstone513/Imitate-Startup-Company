using System.Collections.Generic;
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

    private void Start()
    {
        nameTable = CSVReader.Read("NameTable");
        tableLength = nameTable.Count;
    }

    public int tryCount = 100000;
    public int rangeMin = 1;
    public int rangeMax = 10;

    private void TestND()
    {
        int[] result = NormalDistribution.GetRange(tryCount, rangeMin, rangeMax);
        int size = result.Length;

        for (int i = 0; i < size; i++)
            Debug.Log($"{i + rangeMin}:{result[i]}");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            TestND();
    }

    private int GetNormalDistributionNumber(int min, int max)
    {
        return Mathf.RoundToInt(NormalDistribution.GetData(min, max));
    }

    private string CreateName()
    {
        Gender gender = (Gender)Random.Range(0, 2);
        int first = Random.Range(0, tableLength);
        int last = Random.Range(0, tableLength);

        string name;
        if (gender == Gender.Male)
            name = $"{nameTable[first]["First"]} {nameTable[last]["Male"]}";
        else
            name = $"{nameTable[first]["First"]} {nameTable[last]["Female"]}";
        return name;
    }
}