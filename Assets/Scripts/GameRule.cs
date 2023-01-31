using UnityEngine;

[CreateAssetMenu(fileName = "GameRule.asset", menuName = "GameRule")]
public class GameRule : ScriptableObject
{
    public int baseWorkloadAmount = 10;
    public int ratioWorkloadAmount = 10;
    [Range(1, 5)]
    public int abilityMin = 1;
    [Range(10, 20)]
    public int abilityMax = 10;

    public int constantSpeed = 5;
    public int constantSkipSpeed = 100;
    public int endYear = 10;
}