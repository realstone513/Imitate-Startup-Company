using UnityEngine;

[CreateAssetMenu(fileName = "GameRule.asset", menuName = "GameRule")]
public class GameRule : ScriptableObject
{
    // Strong
    [Range(10, 25)]
    public int workloadDmgMid = 10;
    [Range(1, 10)]
    public int workloadDmgAmplitude = 5;

    // Dexterity 
    [Range(20, 100)]
    public int baseWorkloadAmount = 30;
    [Range(1, 50)]
    public int ratioWorkloadAmount = 10;

    // Intelligence 
    [Range(0.1f, 0.4f)]
    public float successRateMin = 0.25f;
    [Range(0.6f, 0.9f)]
    public float successRateMax = 0.75f;

    [Range(1, 5)]
    public int abilityMin = 1;
    [Range(10, 20)]
    public int abilityMax = 10;

    [Range(5, 50)]
    public int constantSpeed = 25;
    [Range(50, 400)]
    public int constantSkipSpeed = 250;
    [Range(5, 20)]
    public int endYear = 10;

    public Color planColor = new(0f, 0.4f, 1f, 1f);
    public Color devColor = new(0.78f, 0f, 0f, 1f);
    public Color artColor = new(0f, 0.34f, 0f, 1f);

    public Color[] gradeColors = {
        new Color(0.52f, 0f, 0.69f, 1f),
        new Color(098f, 0.36f, 0f, 1f),
        new Color(0.24f, 0.95f, 0.90f, 1f),
    };
}