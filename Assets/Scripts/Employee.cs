using UnityEngine;

public struct EmployeeBaseAblity
{
    // â�Ƿ�, ���Ǽ�, �ֵ���
    public int creativity;
    public int conscientiousness;
    public int scrupulosity;

    public int creativityLimit;
    public int conscientiousnessLimit;
    public int scrupulosityLimit;

    public int maxHP;
    public int currentHP;

    public void SetLimit(int cre, int con, int scr)
    {
        creativityLimit = cre;
        conscientiousnessLimit = con;
        scrupulosityLimit = scr;
    }

    public EmployeeBaseAblity(
        (int min, int max) range, int cre, int con, int scr, int hp)
    {
        creativityLimit = cre;
        conscientiousnessLimit = con;
        scrupulosityLimit = scr;

        creativity = Random.Range(range.min, cre + 1);
        conscientiousness = Random.Range(range.min, con + 1);
        scrupulosity = Random.Range(range.min, scr + 1);

        maxHP = hp;
        currentHP = maxHP;
    }
}

public enum States
{
    None = -1,
    GoToWork,
    Working,
    OffWork,
    Vacation,
    Education
}

public class Employee : MonoBehaviour
{
    private string empName;
    private EmployeeBaseAblity ability;
    private float workload;
    private float currentWorkload;
    private float timer;
    private float duration;
    private States state;

    public void SetInit(string _name, EmployeeBaseAblity _ability)
    {
        empName = _name;
        ability = _ability;
        state = States.None;

        TestPrint();
    }

    public void TestPrint()
    {
        Debug.Log($"{name} " +
            $"â�Ǽ�: {ability.creativity}/{ability.creativityLimit} " +
            $"���Ǽ�: {ability.conscientiousness}/{ability.conscientiousnessLimit} " +
            $"�ֵ���: {ability.scrupulosity}/{ability.scrupulosityLimit}");
    }
}