namespace Realstone
{
    using TMPro;
    using UnityEngine;

    public class EmployeeDetail : MonoBehaviour
    {
        public TextMeshProUGUI hpText;
        public TextMeshProUGUI successText;
        public TextMeshProUGUI greatText;
        public TextMeshProUGUI cumulateText;
        public TextMeshProUGUI hiredDateText;
        public TextMeshProUGUI oneTimeWorkloadAmountText;
        public TextMeshProUGUI experienceText;
        public TextMeshProUGUI workloadPerHourText;

        public void SetInfo(Employee employee)
        {
            hpText.text = $"{employee.ability.hp.current}/{employee.ability.hp.limit}";
            successText.text = $"{employee.GetSuccessRate() * 100:.00}%";
            greatText.text = $"{employee.GetGreatRate() * 100:.00}%";
            cumulateText.text = $"{employee.GetCumulateWorkload()}";
            hiredDateText.text = $"{employee.hiredDate.GetString()}";
            (float, float) range = employee.GetWorkloadAmountRange();
            oneTimeWorkloadAmountText.text = $"{range.Item1} ~ {range.Item2}";
            experienceText.text = $"{employee.experience}";
            workloadPerHourText.text = $"{employee.GetExpectedValue()}";
        }
    }
}