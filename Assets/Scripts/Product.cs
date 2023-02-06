using TMPro;
using UnityEngine;
using UnityEngine.UI;

public struct Plan
{
    public (int current, int max) plan;
    public (int current, int max) dev;
    public (int current, int max) art;

    public (float current, float max) originality;
    public (float current, float max) graphic;
    public (float current, float max) completeness;

    public float originalityResult;
    public float graphicResult;
    public float completenessResult;

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

    /*public float CheckProgress()
    {
        float p = Utils.GetTupleRatio(plan);
        float d = Utils.GetTupleRatio(develop);
        float a = Utils.GetTupleRatio(art);
        return (p + d + a) / 3;
    }

    public float Evaluate()
    {
        originalityResult = Utils.GetTupleRatio(originality);
        graphicResult = Utils.GetTupleRatio(graphic);
        completenessResult = Utils.GetTupleRatio(completeness);
        return (originalityResult + graphicResult + completenessResult) / 3;
    }*/

    public string GetPlanString()
    {
        return ($"��ȹ: {plan} ����: {dev} ��Ʈ: {art}");
    }
}

public class Product : MonoBehaviour
{
    public Plan prodPlan;
    private string productName;

    public TextMeshProUGUI title;
    public Scrollbar planBar;
    public Scrollbar devBar;
    public Scrollbar artBar;


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
        //Debug.Log($"��ȹ:{prodPlan.plan}\n����:{prodPlan.dev}\n��Ʈ:{prodPlan.art}");
        //description.text = $"��ȹ:\n{prodPlan.plan}\n����:\n{prodPlan.dev}\n��Ʈ:\n{prodPlan.art}";
        if (planBar.size == 1f && devBar.size == 1f && artBar.size == 1f)
            CompletePlan();
    }

    private void CompletePlan()
    {
        Debug.Log("Complete");
    }

    public void PrintPlan()
    {
        Debug.Log($"���� �̸�: {productName} {prodPlan.GetPlanString()}");
    }
}