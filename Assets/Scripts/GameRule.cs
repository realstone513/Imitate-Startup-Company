using UnityEngine;

[CreateAssetMenu(fileName = "GameRule.asset", menuName = "GameRule")]
public class GameRule : ScriptableObject
{
    [Range(1, 20)]
    public int baseWorkloadAmount = 10;
    [Range(1, 20)]
    public int ratioWorkloadAmount = 10;
    [Range(1, 5)]
    public int abilityMin = 1;
    [Range(10, 20)]
    public int abilityMax = 10;

    [Range(5, 25)]
    public int constantSpeed = 5;
    [Range(50, 250)]
    public int constantSkipSpeed = 100;
    [Range(5, 20)]
    public int endYear = 10;
}