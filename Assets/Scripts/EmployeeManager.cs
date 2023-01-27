using System.Collections.Generic;
using UnityEngine;

public enum Gender
{
    Male,
    Female,
}

public enum EmployeeRating
{
    Beginner,
    Intermediate,
    Expert,
}

public class EmployeeManager : MonoBehaviour
{
    public List<GameObject> employeeBeginners;
    public List<GameObject> employeeIntermediates;
    public List<GameObject> employeeExperts;
    private List<Dictionary<string, string>> nameTable = new();
    private List<GameObject> employees = new();
    private int tableLength;
    private Vector3 spawnPosition;

    private void Awake()
    {
        nameTable = CSVReader.Read("NameTable");
        tableLength = nameTable.Count;
        spawnPosition = new Vector3(0, -10, 0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            int random = Random.Range(0, employeeBeginners.Count);
            GameObject employee =
                Instantiate(employeeBeginners[random], gameObject.transform);
            employees.Add(employee);
            Employee emp = employee.AddComponent<Employee>();
            emp.SetInit(CreateName(),
                CreateEmployeeBaseAbility(EmployeeRating.Beginner));
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            int random = Random.Range(0, employeeIntermediates.Count);
            GameObject employee =
                Instantiate(employeeIntermediates[random], gameObject.transform);
            employees.Add(employee);
            Employee emp = employee.AddComponent<Employee>();
            emp.SetInit(CreateName(),
                CreateEmployeeBaseAbility(EmployeeRating.Intermediate));
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            int random = Random.Range(0, employeeExperts.Count);
            GameObject employee =
                Instantiate(employeeExperts[random], gameObject.transform);
            employees.Add(employee);
            Employee emp = employee.AddComponent<Employee>();
            emp.SetInit(CreateName(),
                CreateEmployeeBaseAbility(EmployeeRating.Expert));
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            foreach (GameObject emp in employees)
            {
                emp.GetComponent<Employee>().TestPrint();
            }
        }
    }

    private string CreateName()
    {
        Gender gender = (Gender)Random.Range(0, 2);
        int first = Random.Range(0, tableLength);
        int last = Random.Range(0, tableLength);

        string name = (gender == Gender.Male) ?
            $"{nameTable[first]["First"]} {nameTable[last]["Male"]}" :
            $"{nameTable[first]["First"]} {nameTable[last]["Female"]}";
        return name;
    }

    private EmployeeBaseAblity CreateEmployeeBaseAbility(EmployeeRating rating)
    {
        var range = rating switch
        {
            EmployeeRating.Intermediate => (2, 8),
            EmployeeRating.Expert => (4, 10),
            _ => (0, 6),
        };
        EmployeeBaseAblity ablity =
            new(range,
            GetNormalNumber(range),
            GetNormalNumber(range),
            GetNormalNumber(range),
            GetNormalNumber(5000, 10000));
        return ablity;
    }

    private int GetNormalNumber((int min, int max) range)
    {
        return GetNormalNumber(range.min, range.max);
    }

    private int GetNormalNumber(int min, int max)
    {
        return Mathf.RoundToInt(NormalDistribution.GetData(min, max));
    }

    /*public int tryCount = 100000;
    public int rangeMin = 1;
    public int rangeMax = 10;

    private void TestND()
    {
        Dictionary<int, int> result = NormalDistribution.GetRange(tryCount, rangeMin, rangeMax);

        var sort = result.OrderBy(x => x.Key);
        foreach (var i in sort)
            Debug.Log(i);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            TestND();
    }*/
}