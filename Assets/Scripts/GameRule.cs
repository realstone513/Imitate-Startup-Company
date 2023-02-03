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
    public int baseWorkloadAmount = 60;
    public int extraWorkloadAmount = 10;

    // Intelligence 
    public float successRateMin = 0.25f;
    public float successRateMax = 0.75f;
    public float greatRatio = 0.25f;

    [Range(5, 50)]
    public int constantSpeed = 25;
    [Range(50, 400)]
    public int constantSkipSpeed = 250;
    [Range(5, 20)]
    public int endYear = 10;

    [Range(2400, 3600)]
    public int averageSalary = 3000;

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
}