namespace Realstone
{
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class EmployeeAssignRow : MonoBehaviour
    {
        public TextMeshProUGUI eType;
        public TextMeshProUGUI eName;
        public TextMeshProUGUI grade;
        public TextMeshProUGUI hired;
        public Button select;
        private GameObject employeePrefab;

        public void SetInit(GameObject gameObject)
        {
            GameRule rule = GameManager.instance.gameRule;
            Employee employee = gameObject.GetComponent<Employee>();
            employeePrefab = gameObject;

            switch (employee.eType)
            {
                case WorkType.Planner:
                    eType.text = "기획";
                    break;

                case WorkType.Developer:
                    eType.text = "개발";
                    break;

                case WorkType.Artist:
                    eType.text = "아트";
                    break;
            }
            eType.color = rule.typeColors[(int)employee.eType];

            eName.text = employee.empName;

            switch (employee.grade)
            {
                case EmployeeGrade.Beginner:
                    grade.text = "입문";
                    break;
                case EmployeeGrade.Intermediate:
                    grade.text = "중급";
                    break;
                case EmployeeGrade.Expert:
                    grade.text = "전문";
                    break;
            }
            grade.color = rule.gradeColors[(int)employee.grade];

            hired.text = employee.hiredDate.GetString();
            select.onClick.AddListener(AssignOnDesk);
            select.onClick.AddListener(WindowManager.instance.AllClose);
        }

        private void AssignOnDesk()
        {
            employeePrefab.GetComponent<Employee>().AssignOnDesk(
                GameManager.instance.GetCurrentDesk());
            GameManager.instance.employeeManager.MoveToAssign(employeePrefab);
        }
    }
}