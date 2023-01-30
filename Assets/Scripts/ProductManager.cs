using UnityEngine;

public class ProductManager : MonoBehaviour
{
    public Transform content;
    Product prod;

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
            prod?.PrintPlan();
        }
    }

    public void NewProduct()
    {
        prod = new();
        prod.SetPlan(10, 10, 10);
    }
}