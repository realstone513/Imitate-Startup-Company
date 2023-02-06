using System.Collections.Generic;
using UnityEngine;

public class ProductManager : MonoBehaviour
{
    public Transform content;
    public GameObject productPrefab;
    public List<Product> products;

    public void NewProduct(string name, int plan, int dev, int art)
    {
        GameObject tempProd = Instantiate(productPrefab, content);
        Product prd = tempProd.GetComponent<Product>();
        products.Add(prd);
        prd.SetPlan(name, plan, dev, art);
    }
    
    private int FindNeedWork(WorkType type)
    {
        int count = products.Count;
        if (count == 0)
            return -1;
        for (int i = 0; i < count; i++)
        {
            float progress = products[i].prodPlan.GetProgressByType(type);
            if (progress < 1f)
                return i;
        }
        return -1;
    }

    public bool IncreasePlan(int amount)
    {
        int idx = FindNeedWork(WorkType.Planner);
        if (idx < 0)
            return false;
        products[idx].prodPlan.plan.current += amount;
        if (products[idx].prodPlan.plan.current > products[idx].prodPlan.plan.max)
            products[idx].prodPlan.plan.current = products[idx].prodPlan.plan.max;
        products[idx].UpdatePlan();
        return true;
    }

    public bool IncreaseDev(int amount)
    {
        int idx = FindNeedWork(WorkType.Developer);
        if (idx < 0)
            return false;
        products[idx].prodPlan.dev.current += amount;
        if (products[idx].prodPlan.dev.current > products[idx].prodPlan.dev.max)
            products[idx].prodPlan.dev.current = products[idx].prodPlan.dev.max;
        products[idx].UpdatePlan();
        return true;
    }

    public bool IncreaseArt(int amount)
    {
        int idx = FindNeedWork(WorkType.Artist);
        if (idx < 0)
            return false;
        products[idx].prodPlan.art.current += amount;
        if (products[idx].prodPlan.art.current > products[idx].prodPlan.art.max)
            products[idx].prodPlan.art.current = products[idx].prodPlan.art.max;
        products[idx].UpdatePlan();
        return true;
    }
}