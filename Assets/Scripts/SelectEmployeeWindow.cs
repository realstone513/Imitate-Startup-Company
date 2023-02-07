using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectEmployeeWindow : GenericWindow
{
    public GameObject info;
    public GameObject detail;
    private Employee currentEmployee;
    public Button bonusButton;
    private int bonus;

    private void OnEnable()
    {
        currentEmployee = GameManager.instance.GetCurrentDesk().GetOwner();
        info.GetComponent<EmployeeInfo>().SetInfo(currentEmployee);
        detail.GetComponent<EmployeeDetail>().SetInfo(currentEmployee);
        bonusButton.interactable = true;
        bonus = (int)currentEmployee.RequireBonus();
        bonusButton.GetComponentInChildren<TextMeshProUGUI>().text = $"보너스: {bonus}";
    }


    public void FireEmployee()
    {
        GameManager.instance.employeeManager.FireEmployee(currentEmployee.gameObject);
        Close();
    }

    public void GiveBonus()
    {
        currentEmployee.GetBonus();
        GameManager.instance.TranslateGameMoney(bonus);
        bonus = (int)currentEmployee.RequireBonus();
        bonusButton.GetComponentInChildren<TextMeshProUGUI>().text = $"보너스: {bonus}";
        detail.GetComponent<EmployeeDetail>().SetInfo(currentEmployee);
        bonusButton.interactable = false;
    }
}