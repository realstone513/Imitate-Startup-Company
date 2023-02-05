using TMPro;
using UnityEngine;

public class EmployeeInfo : MonoBehaviour
{
    public TextMeshProUGUI type;
    public TextMeshProUGUI employeeName;
    public TextMeshProUGUI grade;
    public TextMeshProUGUI str;
    public TextMeshProUGUI dex;
    public TextMeshProUGUI intelligent;
    public TextMeshProUGUI salary;

    public void SetInfo(Employee employee)
    {
        GameRule rule = GameManager.instance.gameRule;

        switch (employee.eType)
        {
            case WorkType.Planner:
                type.text = "��ȹ";
                break;
            case WorkType.Developer:
                type.text = "����";
                break;
            case WorkType.Artist:
                type.text = "��Ʈ";
                break;
            case WorkType.Player:
                type.text = "��ǥ";
                break;
        }
        type.color = rule.typeColors[(int)employee.eType];

        employeeName.text = employee.empName;

        switch (employee.rating)
        {
            case EmployeeRating.Beginner:
                grade.text = "�Թ�";
                break;
            case EmployeeRating.Intermediate:
                grade.text = "�߱�";
                break;
            case EmployeeRating.Expert:
                grade.text = "����";
                break;
        }
        grade.color = rule.gradeColors[(int)employee.rating];

        str.text = $"{employee.ability.strong}";
        dex.text = $"{employee.ability.dexterity}";
        intelligent.text = $"{employee.ability.intelligence}";
        salary.text = $"{employee.salary}";
        //hired.text = $"{Utils.GetNumberFromDate(employee.hiredDate)}��";
    }
}