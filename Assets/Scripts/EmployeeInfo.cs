using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EmployeeInfo : MonoBehaviour
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
        select.onClick.AddListener(SetPosition);
    }

    private void SetPosition()
    {
        employeePrefab.transform.position = GameManager.instance.GetCurrentDesk().transform.position;
    }
}