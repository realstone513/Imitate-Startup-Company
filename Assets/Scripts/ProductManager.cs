using UnityEngine;

public class ProductManager : MonoBehaviour
{
    public static ProductManager instance;
    public Transform content;
    Product prod;

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

    private void Start()
    {
        prod = null;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            //Instantiate(product, content.transform);
            if (prod == null)
                NewProduct();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            prod?.prodPlan.PrintPlan();
        }
    }

    public void NewProduct()
    {
        prod = new();
        prod.SetPlan(20, 20, 20);
    }
    
    public bool IncreasePlan()
    {
        if (prod == null)
            return false;

        prod.prodPlan.plan.current++;
        Debug.Log($"Plan: {prod.prodPlan.plan}");
        return true;
    }

    public bool IncreaseDev()
    {
        if (prod == null)
            return false;

        prod.prodPlan.develop.current++;
        Debug.Log($"Dev: {prod.prodPlan.develop}");
        return true;
    }

    public bool IncreaseArt()
    {
        if (prod == null)
            return false;

        prod.prodPlan.art.current++;
        Debug.Log($"Art: {prod.prodPlan.art}");
        return true;
    }
}