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
                    eType.text = "��ȹ";
                    break;

                case WorkType.Developer:
                    eType.text = "����";
                    break;

                case WorkType.Artist:
                    eType.text = "��Ʈ";
                    break;
            }
            eType.color = rule.typeColors[(int)employee.eType];

            eName.text = employee.empName;

            switch (employee.grade)
            {
                case EmployeeGrade.Beginner:
                    grade.text = "�Թ�";
                    break;
                case EmployeeGrade.Intermediate:
                    grade.text = "�߱�";
                    break;
                case EmployeeGrade.Expert:
                    grade.text = "����";
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