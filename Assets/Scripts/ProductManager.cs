using System.Collections.Generic;
using UnityEngine;

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
        prd.SetPlan(20, 20, 20);
    }
    
    public bool IncreasePlan()
    {
        products[0].prodPlan.plan.current++;
        Debug.Log($"Plan: {products[0].prodPlan.plan}");
        return true;
    }

    public bool IncreaseDev()
    {
        products[0].prodPlan.develop.current++;
        Debug.Log($"Dev: {products[0].prodPlan.develop}");
        return true;
    }

    public bool IncreaseArt()
    {
        products[0].prodPlan.art.current++;
        Debug.Log($"Art: {products[0].prodPlan.art}");
        return true;
    }
}