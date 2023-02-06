using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameRule.asset", menuName = "GameRule")]
public class GameRule : ScriptableObject
{
    public int abilityMin = 1;
    public int abilityMax = 10;

    // Strong
    public int workloadDmgMid = 12;
    public float workloadDmgAmplitude = 0.4f;
    public int constantStrongValue = 2;

    // Dexterity 
    public int baseWorkloadAmount = 30;
    public int extraWorkloadAmount = 5;

    // Intelligence 
    public float successRateMin = 0.25f;
    public float successRateMax = 0.75f;
    public float greatRatio = 0.25f;

    // Health Point
    public int hpMin = 5000;
    public int hpMax = 10000;
    public int workConsumtionConstant = 5;
    public int offworkRecoveryConstant = 2;
    public int vacationRecoveryConstant = 3;

    public int requireExp = 500;

    [Range(5, 50)]
    public int constantSpeed = 25;
    [Range(50, 400)]
    public int constantSkipSpeed = 250;
    [Range(5, 20)]
    public int endYear = 10;
    [Range(5000, 50000)]
    public int seedMoney = 10000;

    [Range(10, 100)]
    public int jobOfferCostBeginner = 10;
    [Range(100, 1000)]
    public int jobOfferCostIntermediate = 500;
    [Range(1000, 10000)]
    public int jobOfferCostExpert = 3000;

    public List<int> averageSalary = new() { 3000, 5500, 8000 };
    public List<int> planAmountChart = new() { 200, 1000, 3000, 10000 };
    public float salaryRangeRatio = 0.2f;

    public Color[] typeColors =
    {
        new (0f, 0.4f, 1f, 1f),
        new (0.78f, 0f, 0f, 1f),
        new (0f, 0.34f, 0f, 1f),
        new (0.24f, 0.95f, 0.90f, 1f),
    };

    public Color[] gradeColors = {
        new (0.52f, 0f, 0.69f, 1f),
        new (098f, 0.36f, 0f, 1f),
        new (0.24f, 0.95f, 0.90f, 1f),
    };

    public Color[] productColors = { Color.white };
}