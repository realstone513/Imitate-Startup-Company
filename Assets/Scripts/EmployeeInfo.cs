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
                type.text = "기획";
                break;
            case WorkType.Developer:
                type.text = "개발";
                break;
            case WorkType.Artist:
                type.text = "아트";
                break;
            case WorkType.Player:
                type.text = "대표";
                break;
        }
        type.color = rule.typeColors[(int)employee.eType];

        employeeName.text = employee.empName;

        switch (employee.rating)
        {
            case EmployeeRating.Beginner:
                grade.text = "입문";
                break;
            case EmployeeRating.Intermediate:
                grade.text = "중급";
                break;
            case EmployeeRating.Expert:
                grade.text = "전문";
                break;
        }
        grade.color = rule.gradeColors[(int)employee.rating];

        str.text = $"{employee.ability.strong}";
        dex.text = $"{employee.ability.dexterity}";
        intelligent.text = $"{employee.ability.intelligence}";
        salary.text = $"{employee.salary}";
        //hired.text = $"{Utils.GetNumberFromDate(employee.hiredDate)}주";
    }
}