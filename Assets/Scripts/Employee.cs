using System.Collections;
using System.Collections.Generic;
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

public enum EmployeeState
{
    GoToWork,
    Working,
    OffWork,
    Vacation,
    Education
}

public class Employee
{
    private string name;
    private EmployeeBaseAblity ability;

    public Employee(string _name, EmployeeBaseAblity _ability)
    {
        name = _name;
        ability = _ability;
        TestPrint();
    }

    private void TestPrint()
    {
        Debug.Log($"{name} " +
            $"â�Ǽ�: {ability.creativity}/{ability.creativityLimit} " +
            $"���Ǽ�: {ability.conscientiousness}/{ability.conscientiousnessLimit} " +
            $"�ֵ���: {ability.scrupulosity}/{ability.scrupulosityLimit}");
    }
}