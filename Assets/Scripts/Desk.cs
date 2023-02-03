using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Desk : MonoBehaviour
{
    private Employee owner;
    public GameObject chair;

    public void SetOwner(Employee employee)
    {
        if (owner != null)
        {
            EmployeeManager.instance.MoveToUnassign(owner.gameObject);
            owner.UnassignOnDesk();
        }
        owner = employee;
    }

    public void RemoveOwner()
    {
        owner = null;
    }
}