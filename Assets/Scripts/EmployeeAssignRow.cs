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
        eType.text = employee.eType.ToString();
        eName.text = employee.empName;
        rating.text = employee.rating.ToString();
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