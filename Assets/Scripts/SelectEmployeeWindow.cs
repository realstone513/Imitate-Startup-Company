namespace Realstone
{
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
            SetBonusButton();
            bonusButton.interactable = true;
        }

        public void FireEmployee()
        {
            GameManager.instance.employeeManager.FireEmployee(currentEmployee.gameObject);
            Close();
        }

        public void GiveBonus()
        {
            currentEmployee.GetBonus();
            GameManager.instance.TranslateGameMoney(-bonus);
            SetBonusButton();
            detail.GetComponent<EmployeeDetail>().SetInfo(currentEmployee);
            bonusButton.interactable = false;
        }

        private void SetBonusButton()
        {
            bonus = (int)currentEmployee.RequireBonus();
            bonusButton.GetComponentInChildren<TextMeshProUGUI>().text = $"º¸³Ê½º: {bonus}";
        }
    }
}