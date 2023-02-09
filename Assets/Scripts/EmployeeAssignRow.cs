namespace Realstone
{
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class EmployeeAssignRow : MonoBehaviour
    {
        public TextMeshProUGUI eType;
        public TextMeshProUGUI eName;
        public TextMeshProUGUI rating;
        public TextMeshProUGUI hired;
        public Button select;
        private GameObject employeePrefab;

        public void SetInit(GameObject gameObject)
        {
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

            eName.text = employee.empName;

            switch (employee.rating)
            {
                case EmployeeRating.Beginner:
                    rating.text = "�Թ�";
                    break;
                case EmployeeRating.Intermediate:
                    rating.text = "�߱�";
                    break;
                case EmployeeRating.Expert:
                    rating.text = "����";
                    break;
            }

            hired.text = employee.hiredDate.GetString();
            select.onClick.AddListener(WindowManager.instance.AllClose);
            select.onClick.AddListener(AssignOnDesk);
        }

        private void AssignOnDesk()
        {
            employeePrefab.GetComponent<Employee>().AssignOnDesk(
                GameManager.instance.GetCurrentDesk());
            GameManager.instance.employeeManager.MoveToAssign(employeePrefab);
        }
    }
}