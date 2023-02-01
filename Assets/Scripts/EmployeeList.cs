using System.Collections.Generic;
using UnityEngine;

public class EmployeeList : GenericWindow
{
    public GameObject infoPrefab;
    public Transform content;
    private List<GameObject> employeeInfos = new();

    private void OnEnable()
    {
        EmployeeManager em = EmployeeManager.instance;

        List<GameObject> unassign = em.GetUnassign();
        foreach (GameObject emp in unassign)
        {
            GameObject info = Instantiate(infoPrefab, content);
            info.GetComponent<EmployeeInfo>().SetInfo(emp.GetComponent<Employee>());
            employeeInfos.Add(info);
        }

        List<GameObject> assign = em.GetAssign();
        foreach (GameObject emp in assign)
        {
            GameObject info = Instantiate(infoPrefab, content);
            info.GetComponent<EmployeeInfo>().SetInfo(emp.GetComponent<Employee>());
            employeeInfos.Add(info);
        }
    }

    private void OnDisable()
    {
        foreach (GameObject info in employeeInfos)
        {
            Destroy(info);
        }
        employeeInfos.Clear();
    }
}