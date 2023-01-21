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

    private void Start()
    {
        nameTable = CSVReader.Read("NameTable");
        tableLength = nameTable.Count;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CreateName((Gender) Random.Range(0, 1));
        }
    }

    private string CreateName(Gender gender)
    {
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