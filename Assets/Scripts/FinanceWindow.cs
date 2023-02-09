namespace Realstone
{
    using TMPro;
    using UnityEngine;

    public class FinanceWindow : GenericWindow
    {
        public TextMeshProUGUI lossList;
        public TextMeshProUGUI lossAmount;
        public TextMeshProUGUI profitList;
        public TextMeshProUGUI profitAmount;
        public TextMeshProUGUI balance;
        public TextMeshProUGUI nextMonth;

        private void OnEnable()
        {
            GameManager gm = GameManager.instance;
            balance.text = $"{gm.money}";
            lossList.text = "";
            lossAmount.text = "";
            int lossSum = 0;
            foreach (var elem in gm.financeLossDictionary)
            {
                lossList.text += $"{elem.Key}\n";
                lossAmount.text += $"{elem.Value}\n";
                lossSum += elem.Value;
            }
            lossList.text += $"합계 :";
            lossAmount.text += $"{lossSum}";

            profitList.text = "";
            profitAmount.text = "";
            int profitSum = 0;
            foreach (var elem in gm.financeProfitDictionary)
            {
                profitList.text += $"{elem.Key}\n";
                profitAmount.text += $"{elem.Value}\n";
                profitSum += elem.Value;
            }
            profitList.text += $"합계 :";
            profitAmount.text += $"{profitSum}";
            int total = profitSum - lossSum;
            nextMonth.color = total > 0 ? Color.green : Color.red;
            nextMonth.text = $"{total}";
        }
    }
}