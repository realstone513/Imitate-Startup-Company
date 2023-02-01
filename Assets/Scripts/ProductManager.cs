using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ProductManager : MonoBehaviour
{
    public static ProductManager instance;
    public Transform content;
    public GameObject productPrefab;
    public List<Product> products;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            NewProduct();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            foreach (Product product in products)
                product.prodPlan.PrintPlan();
        }
    }

    public void NewProduct()
    {
        GameObject tempProd = Instantiate(productPrefab, content);
        Product prd = tempProd.GetComponent<Product>();
        products.Add(prd);
        prd.SetPlan(2000, 2000, 2000);
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
        // Debug.Log($"Plan{idx}: {products[idx].prodPlan.plan}");
        return true;
    }

    public bool IncreaseDev(int amount)
    {
        int idx = FindNeedWork(WorkType.Developer);
        if (idx < 0)
            return false;
        products[idx].prodPlan.develop.current += amount;
        if (products[idx].prodPlan.develop.current > products[idx].prodPlan.develop.max)
            products[idx].prodPlan.develop.current = products[idx].prodPlan.develop.max;
        // Debug.Log($"Dev{idx}: {products[idx].prodPlan.develop}");
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
        // Debug.Log($"Art{idx}: {products[idx].prodPlan.art}");
        return true;
    }
}