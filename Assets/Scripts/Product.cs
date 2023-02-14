namespace Realstone
{
    using System.Threading;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public struct Plan
    {
        public (int current, int max) plan;
        public (int current, int max) dev;
        public (int current, int max) art;

        public (float current, float count) originality;
        public (float current, float count) completeness;
        public (float current, float count) graphic;

        public float originalityResult;
        public float completenessResult;
        public float graphicResult;

        public void Init(int _plan, int _dev, int _art)
        {
            plan = (0, _plan);
            dev = (0, _dev);
            art = (0, _art);

            originality = (0f, 0f);
            graphic = (0f, 0f);
            completeness = (0f, 0f);
        }

        public float GetProgressByType(WorkType type)
        {
            float result = 0f;
            switch (type)
            {
                case WorkType.Planner:
                    result = Utils.GetTupleRatio(plan);
                    break;
                case WorkType.Artist:
                    result = Utils.GetTupleRatio(art);
                    break;
                case WorkType.Developer:
                    result = Utils.GetTupleRatio(dev);
                    break;
                default:
                    break;
            }
            return result;
        }

        public float Evaluate() // 0.00 ~ 10.00
        {
            originalityResult = Utils.GetTupleRatio(originality);
            graphicResult = Utils.GetTupleRatio(graphic);
            completenessResult = Utils.GetTupleRatio(completeness);
            return (originalityResult + graphicResult + completenessResult) / 3 * 10;
        }

        public string GetPlanString()
        {
            return $"기획: {plan} 개발: {dev} 아트: {art}";
        }
    }

    public class Product : MonoBehaviour
    {
        public Plan prodPlan;
        private string productName;

        public TextMeshProUGUI title;
        public GameObject planObject;
        public GameObject serviceObject;
        public Scrollbar planBar;
        public Scrollbar devBar;
        public Scrollbar artBar;
        public TextMeshProUGUI evaluationPointText;
        public TextMeshProUGUI numberOfUsersText;
        public TextMeshProUGUI monthlyIncomeText;
        private bool onService = false;
        private Date serviceStartDate;
        private (float current, float duration) timer = (0f, 25f);
        private GameManager gm;
        private float evaluationPoint;
        private int numberOfUsers;
        private float monthlyIncome;

        private void Start()
        {
            gm = GameManager.instance;
            var colors = gm.gameRule.productColors;
            gameObject.GetComponent<Image>().color = colors[Random.Range(0, colors.Length)];
            planObject.SetActive(true);
            serviceObject.SetActive(false);
            numberOfUsers = 0;
            monthlyIncome = 0;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                CompletePlan();
            }

            if (!onService)
                return;

            timer.current += gm.deltaTime;
            if (timer.current >= timer.duration)
            {
                timer.current = 0f;
                int serviceDate = Utils.GetNumberFromDate(gm.GetToday()) - Utils.GetNumberFromDate(serviceStartDate);
                if (serviceDate < evaluationPoint * 10)
                {
                    UpdateUserAndIncome(10f);
                }
                if (serviceDate < evaluationPoint * 20)
                {
                    UpdateUserAndIncome(5f);
                }
                else
                {
                    UpdateUserAndIncome(-1f);
                }
                numberOfUsersText.text = $"{numberOfUsers}";
                monthlyIncomeText.text = $"{monthlyIncome}";
            }

        }

        private void UpdateUserAndIncome(float growthRate)
        {
            int adder = (int)NormalDistribution.GetData(0, evaluationPoint * growthRate);
            numberOfUsers += adder;
            if (numberOfUsers <= 0)
                numberOfUsers = 0;
            float beforeIncome = monthlyIncome;
            monthlyIncome = (adder * 0.1f);

            if (beforeIncome > monthlyIncome)
                ClearDictionaryElem();

            if (monthlyIncome >= 0)
                gm.financeProfitDictionary[productName] = (int)monthlyIncome;
            else
                gm.financeLossDictionary[productName] = (int)monthlyIncome;
        }

        public void SetPlan(string name, int plan, int dev, int art)
        {
            productName = name;
            prodPlan.Init(plan, dev, art);
            title.text = productName;
            UpdatePlan();
        }

        public void UpdatePlan()
        {
            planBar.size = Utils.GetTupleRatio(prodPlan.plan);
            devBar.size = Utils.GetTupleRatio(prodPlan.dev);
            artBar.size = Utils.GetTupleRatio(prodPlan.art);
            if (planBar.size == 1f && devBar.size == 1f && artBar.size == 1f)
                CompletePlan();
        }

        private void CompletePlan()
        {
            planObject.SetActive(false);
            serviceObject.SetActive(true);
            onService = true;
            serviceStartDate = gm.GetToday();
            evaluationPoint = prodPlan.Evaluate();
            evaluationPointText.text = $"{evaluationPoint:0.00}";
            numberOfUsersText.text = "0";
            monthlyIncomeText.text = "0";
            gm.financeProfitDictionary.Add(productName, (int)monthlyIncome);
        }

        public void PrintPlan()
        {
            Debug.Log($"게임 이름: {productName} {prodPlan.GetPlanString()}");
        }

        public void EndOfService()
        {
            gm.productManager.products.Remove(this);
            ClearDictionaryElem();
            Destroy(gameObject);
        }

        public void ClearMonthlyIncome()
        {
            monthlyIncome = 0;
            monthlyIncomeText.text = "0";
            ClearDictionaryElem();
        }

        private void ClearDictionaryElem()
        {
            if (gm.financeProfitDictionary.ContainsKey(productName))
                gm.financeProfitDictionary.Remove(productName);
            if (gm.financeLossDictionary.ContainsKey(productName))
                gm.financeLossDictionary.Remove(productName);
        }
    }
}