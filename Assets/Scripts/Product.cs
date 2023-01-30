using UnityEngine;

public struct Plan
{
    (int current, int max) plan;
    (int current, int max) develop;
    (int current, int max) art;

    (float current, float max) originality;
    (float current, float max) graphic;
    (float current, float max) completeness;

    float originalityResult;
    float graphicResult;
    float completenessResult;

    public void Init(int _plan, int _dev, int _art)
    {
        plan = (0, _plan);
        develop = (0, _dev);
        art = (0, _art);

        originality = (0f, 0f);
        graphic = (0f, 0f);
        completeness = (0f, 0f);
    }

    public float CheckProgress()
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
    }

    public void PrintPlan()
    {
        Debug.Log($"기획: {plan} 개발: {develop} 아트: {art}");
    }
}

public class Product
{
    private Plan prodPlan;

    public void SetPlan(int plan, int dev, int art)
    {
        prodPlan.Init(plan, dev, art);
    }
    
    public void PrintPlan()
    {
        prodPlan.PrintPlan();
    }

    public float GetProgress()
    {
        return prodPlan.CheckProgress();
    }

    public float GetEvaluate()
    {
        return prodPlan.Evaluate();
    }
}