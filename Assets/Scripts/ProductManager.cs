using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductManager : MonoBehaviour
{
    public GameObject content;
    public GameObject product;
    Product prod;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            //Instantiate(product, content.transform);
            prod = new();
            prod.SetPlan(10, 10, 10);
        }
    }
}