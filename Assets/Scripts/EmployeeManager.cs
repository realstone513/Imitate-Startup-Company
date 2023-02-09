namespace Realstone
{
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
        private List<GameObject> unassign = new();
        private List<GameObject> assign = new();
        private int tableLength;
        private Vector3 employeeSpawnPosition;

        private void Start()
        {
            nameTable = CSVReader.Read("NameTable");
            tableLength = nameTable.Count;
            employeeSpawnPosition = new Vector3(0, -10, 0);
            GameObject player = CreateNewEmployee(EmployeeRating.Expert, WorkType.Player);
            player.GetComponent<Employee>().AssignOnDesk(GameManager.instance.GetCurrentDesk());
            MoveToAssign(player);
        }

        /*private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log("미배치 직원");
                foreach (GameObject emp in unassign)
                {
                    emp.GetComponent<Employee>().TestPrint();
                }
                Debug.Log("배치 된 직원");
                foreach (GameObject emp in assign)
                {
                    emp.GetComponent<Employee>().TestPrint();
                }
            }

            // Update Cheat Key
            if (Input.GetKeyDown(KeyCode.Q))
            {
                AddToUnassign(CreateNewEmployee(EmployeeRating.Beginner, WorkType.Planner));
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                AddToUnassign(CreateNewEmployee(EmployeeRating.Intermediate, WorkType.Planner));
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                AddToUnassign(CreateNewEmployee(EmployeeRating.Expert, WorkType.Planner));
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                AddToUnassign(CreateNewEmployee(EmployeeRating.Beginner, WorkType.Developer));
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                AddToUnassign(CreateNewEmployee(EmployeeRating.Intermediate, WorkType.Developer));
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                AddToUnassign(CreateNewEmployee(EmployeeRating.Expert, WorkType.Developer));
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                AddToUnassign(CreateNewEmployee(EmployeeRating.Beginner, WorkType.Artist));
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                AddToUnassign(CreateNewEmployee(EmployeeRating.Intermediate, WorkType.Artist));
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                AddToUnassign(CreateNewEmployee(EmployeeRating.Expert, WorkType.Artist));
            }
        }*/

        public void MoveToAssign(GameObject select)
        {
            unassign.Remove(select);
            assign.Add(select);
        }

        public void MoveToUnassign(GameObject select)
        {
            assign.Remove(select);
            unassign.Add(select);
        }

        public void AddToUnassign(GameObject select)
        {
            unassign.Add(select);
        }

        public GameObject CreateNewEmployee(EmployeeRating rating, WorkType eType)
        {
            List<GameObject> tempList = rating switch
            {
                EmployeeRating.Intermediate => employeeIntermediates,
                EmployeeRating.Expert => employeeExperts,
                _ => employeeBeginners,
            };
            int random = Random.Range(0, tempList.Count);
            GameObject employee =
                Instantiate(tempList[random], employeeSpawnPosition, Quaternion.identity, gameObject.transform);
            employee.AddComponent<Employee>().SetInit(eType, rating, CreateName(), CreateEmployeeBaseAbility(rating));
            return employee;
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
            int min = GameManager.instance.gameRule.abilityMin;
            int max = GameManager.instance.gameRule.abilityMax;
            int mid = (int)Utils.GetAverage(min, max);
            int minmid = (int)Utils.GetAverage(min, mid);

            // min 1, max 10 - 1~6, 3~8, 5~10
            var range = rating switch
            {
                EmployeeRating.Intermediate => (minmid, minmid + mid),
                EmployeeRating.Expert => (mid, max),
                _ => (min, min + mid),
            };
            GameRule rule = GameManager.instance.gameRule;
            EmployeeBaseAblity ability =
                new(range,
                GetNormalNumber(range),
                GetNormalNumber(range),
                GetNormalNumber(range),
                GetNormalNumber((rule.hpMin, rule.hpMax)));
            return ability;
        }

        public void GotoWorkTrigger()
        {
            foreach (GameObject employee in assign)
            {
                employee.GetComponent<Employee>().GoToWorkTrigger();
            }
        }

        private int GetNormalNumber((int min, int max) range)
        {
            return Mathf.RoundToInt(NormalDistribution.GetData(range.min, range.max));
        }

        public List<GameObject> GetUnassign()
        {
            return unassign;
        }

        public List<GameObject> GetAssign()
        {
            return assign;
        }

        public void FireEmployee(GameObject gameObject)
        {
            assign.Remove(gameObject);
            GameManager gm = GameManager.instance;
            string key = $"{gameObject.GetComponent<Employee>().empName} 월급";
            gm.TranslateGameMoney(-gm.financeLossDictionary[key]);
            gm.financeLossDictionary.Remove(key);
            Destroy(gameObject);
        }

        // Test Code
        /*public int tryCount = 100000;
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

            foreach (var i in sort)
            {
                Debug.Log(i);
            }

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
        }*/
    }
}