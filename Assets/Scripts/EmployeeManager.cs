namespace Realstone
{
    using System.Collections.Generic;
    using UnityEngine;

    public enum Gender
    {
        Male,
        Female,
    }

    public enum EmployeeGrade
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
            GameObject player = CreateNewEmployee(EmployeeGrade.Expert, WorkType.Player);
            player.GetComponent<Employee>().AssignOnDesk(GameManager.instance.GetCurrentDesk());
            MoveToAssign(player);
        }

        public int GetEmployeeCount()
        {
            // - player
            return assign.Count + unassign.Count - 1;
        }

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

        public GameObject CreateNewEmployee(EmployeeGrade grade, WorkType eType)
        {
            List<GameObject> tempList = grade switch
            {
                EmployeeGrade.Intermediate => employeeIntermediates,
                EmployeeGrade.Expert => employeeExperts,
                _ => employeeBeginners,
            };
            int random = Random.Range(0, tempList.Count);
            GameObject employee =
                Instantiate(tempList[random], employeeSpawnPosition, Quaternion.identity, gameObject.transform);
            employee.AddComponent<Employee>().SetInit(eType, grade, CreateName(), CreateEmployeeBaseAbility(grade));
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

        private EmployeeBaseAblity CreateEmployeeBaseAbility(EmployeeGrade grade)
        {
            int min = GameManager.instance.gameRule.abilityMin;
            int max = GameManager.instance.gameRule.abilityMax;
            int mid = (int)Utils.GetAverage(min, max);
            int minmid = (int)Utils.GetAverage(min, mid);

            // min 1, max 10 - 1~6, 3~8, 5~10
            var range = grade switch
            {
                EmployeeGrade.Intermediate => (minmid, minmid + mid),
                EmployeeGrade.Expert => (mid, max),
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
            string key = $"{gameObject.GetComponent<Employee>().empName} ПљБо";
            gm.TranslateGameMoney(-gm.financeLossDictionary[key]);
            gm.financeLossDictionary.Remove(key);
            Destroy(gameObject);
        }
    }
}