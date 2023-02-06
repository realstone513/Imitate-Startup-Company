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

    public float Evaluate()
    {
        originalityResult = Utils.GetTupleRatio(originality);
        graphicResult = Utils.GetTupleRatio(graphic);
        completenessResult = Utils.GetTupleRatio(completeness);
        return (originalityResult + graphicResult + completenessResult) / 3 * 100;
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
    public TextMeshProUGUI evaluationPoint;
    public TextMeshProUGUI numberOfUsers;
    public TextMeshProUGUI monthlyIncome;

    private void Start()
    {
        var colors = GameManager.instance.gameRule.productColors;
        gameObject.GetComponent<Image>().color = colors[Random.Range(0, colors.Length)];
        planObject.SetActive(true);
        serviceObject.SetActive(false);
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
        //Debug.Log($"기획:{prodPlan.plan}\n개발:{prodPlan.dev}\n아트:{prodPlan.art}");
        //description.text = $"기획:\n{prodPlan.plan}\n개발:\n{prodPlan.dev}\n아트:\n{prodPlan.art}";
        if (planBar.size == 1f && devBar.size == 1f && artBar.size == 1f)
            CompletePlan();
    }

    private void CompletePlan()
    {
        Debug.Log("Complete");
        planObject.SetActive(false);
        serviceObject.SetActive(true);
        evaluationPoint.text = $"{prodPlan.Evaluate():.00}";
    }

    public void PrintPlan()
    {
        Debug.Log($"게임 이름: {productName} {prodPlan.GetPlanString()}");
    }
    
    public void EndOfService()
    {
        GameManager.instance.productManager.products.Remove(this);
        Destroy(gameObject);
    }
}