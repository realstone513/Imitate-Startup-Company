using System.Collections.Generic;
using System.Linq;
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
    public static EmployeeManager instance;

    public List<GameObject> employeeBeginners;
    public List<GameObject> employeeIntermediates;
    public List<GameObject> employeeExperts;
    private List<Dictionary<string, string>> nameTable = new();
    private List<GameObject> unassign = new();
    private List<GameObject> assign = new();
    private int tableLength;
    private Vector3 employeeSpawnPosition;
    public GameObject employeeInfo;
    public Transform employeeInfoSpawnTransform;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        nameTable = CSVReader.Read("NameTable");
        tableLength = nameTable.Count;
        employeeSpawnPosition = new Vector3(0, -10, 0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            CreateNewEmployee(EmployeeRating.Beginner, EmployeeType.Planner);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            CreateNewEmployee(EmployeeRating.Intermediate, EmployeeType.Planner);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            CreateNewEmployee(EmployeeRating.Expert, EmployeeType.Planner);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            CreateNewEmployee(EmployeeRating.Beginner, EmployeeType.Developer);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            CreateNewEmployee(EmployeeRating.Intermediate, EmployeeType.Developer);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            CreateNewEmployee(EmployeeRating.Expert, EmployeeType.Developer);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            CreateNewEmployee(EmployeeRating.Beginner, EmployeeType.Artist);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            CreateNewEmployee(EmployeeRating.Intermediate, EmployeeType.Artist);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            CreateNewEmployee(EmployeeRating.Expert, EmployeeType.Artist);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            foreach (GameObject emp in unassign)
            {
                emp.GetComponent<Employee>().TestPrint();
            }
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            TestND();
        }
    }

    private void MoveList(GameObject select, List<GameObject> start, List<GameObject> dest)
    {
        start.Remove(select);
        dest.Add(select);
    }

    private void CreateNewEmployee(EmployeeRating rating, EmployeeType eType)
    {
        List<GameObject> tempList;
        switch (rating)
        {
            case EmployeeRating.Beginner:
                tempList = employeeBeginners;
                break;
            case EmployeeRating.Intermediate:
                tempList = employeeIntermediates;
                break;
            case EmployeeRating.Expert:
            default:
                tempList = employeeExperts;
                break;
        }

        int random = Random.Range(0, tempList.Count);
        GameObject employee =
            Instantiate(tempList[random], employeeSpawnPosition, Quaternion.identity, gameObject.transform);
        unassign.Add(employee);
        Employee emp = employee.AddComponent<Employee>();
        emp.SetInit(eType, rating, CreateName(), CreateEmployeeBaseAbility(rating));
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
            EmployeeRating.Intermediate => (3, 8),
            EmployeeRating.Expert => (5, 10),
            _ => (1, 6),
        };
        EmployeeBaseAblity ablity =
            new(range,
            GetNormalNumber(range),
            GetNormalNumber(range),
            GetNormalNumber(range),
            GetNormalNumber((5000, 10000)));
        return ablity;
    }

    private int GetNormalNumber((int min, int max) range)
    {
        return Mathf.RoundToInt(NormalDistribution.GetData(range.min, range.max));
    }


    // Test Code
    public int tryCount = 100000;
    public int rangeMin = 1;
    public int rangeMax = 10;

    public int innerSumPoint;

    private void TestND()
    {
        Dictionary<int, int> result = NormalDistribution.GetRange(tryCount, rangeMin, rangeMax);

        int sum = 0;
        int ispIdx = 0;
        int count = 0;
        var sort = result.OrderBy(x => x.Key);
        int[] points = { innerSumPoint, rangeMax - innerSumPoint, sort.Last().Key };
        int before = rangeMin;

        /*foreach (var i in sort)
        {
            Debug.Log(i);
        }*/

        foreach (var i in sort)
        {
            //Debug.Log(i);
            sum += i.Value;
            count++;

            if (count == points[ispIdx])
            {
                Debug.Log($"{before} ~ {count} : {(float)sum / tryCount * 100}");
                ispIdx++;
                before = count;
                sum = 0;
            }
        }
    }
}