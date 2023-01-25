using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Employee
{
    int creativity;
    int conscientiousness;
    int scrupulosity;

    int creativityLimit;
    int conscientiousnessLimit;
    int scrupulosityLimit;

    public Employee()
    {
        creativity = Random.Range(1, 10);
        conscientiousness = Random.Range(1, 10);
        scrupulosity = Random.Range(1, 10);
    }

}