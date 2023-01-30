using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EmployeeInfo : MonoBehaviour
{
    public TextMeshProUGUI eType;
    public TextMeshProUGUI eName;
    public TextMeshProUGUI rating;
    public TextMeshProUGUI hired;

    public void SetInit(Employee employee)
    {
        eType.text = employee.eType.ToString();
        eName.text = employee.empName;
        rating.text = employee.rating.ToString();
        hired.text = employee.hiredDate.GetString();
    }
}