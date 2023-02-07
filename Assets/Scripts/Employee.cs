using System.Collections.Generic;
using System.Data;
using UnityEngine;

public struct EmployeeBaseAblity
{
    public (int current, int limit) strong;
    public (int current, int limit) dexterity;
    public (int current, int limit) intelligence;
    public (float current, float limit) hp;

    public EmployeeBaseAblity(
        (int min, int max) range, int cre, int con, int scr, float hp)
    {
        strong = (Random.Range(range.min, cre + 1), cre);
        dexterity = (Random.Range(range.min, con + 1), con);
        intelligence = (Random.Range(range.min, scr + 1), scr);
        this.hp = (hp, hp);
    }

    public bool CanUpgrade()
    {
        return CanUpgradeStrong() || CanUpgradeDexterity() || CanUpgradeIntelligence();
    }

    private bool CanUpgradeStrong()
    {
        return Utils.GetTupleRatio(strong) != 1;
    }

    private bool CanUpgradeDexterity()
    {
        return Utils.GetTupleRatio(dexterity) != 1;
    }

    private bool CanUpgradeIntelligence()
    {
        return Utils.GetTupleRatio(intelligence) != 1;
    }

    public void Upgrade()
    {
        List<int> canUpgradeIdx = new();

        if (CanUpgradeStrong())
            canUpgradeIdx.Add(0);
        if (CanUpgradeDexterity())
            canUpgradeIdx.Add(1);
        if (CanUpgradeIntelligence())
            canUpgradeIdx.Add(2);

        int idx = canUpgradeIdx[Random.Range(0, canUpgradeIdx.Count)];
        switch (idx)
        {
            case 0:
                strong.current++;
                break;
            case 1:
                dexterity.current++;
                break;
            case 2:
                intelligence.current++;
                break;
        }
    }
}

public enum States
{
    Unassign,
    GoToWork,
    Working,
    OffWork,
    Vacation,
}

public enum WorkType
{
    None = -1,
    Planner,
    Developer,
    Artist,
    Player,
}

public enum WorkDoneType
{
    Fail,
    Success,
    Great,
}

public class Employee : MonoBehaviour
{
    public string empName;
    public EmployeeBaseAblity ability;
    private (float current, float amount) workload;
    public States state;
    public WorkType eType;
    public EmployeeRating rating;
    public Date hiredDate;
    private int hpConstant;
    private int cumulateWorkload;
    public int experience;
    private float successRate;
    private float greatRate;
    private int baseWorkloadAmount;
    private GameManager gm;
    private Desk myDesk;
    public int salary;
    public int fakeSalary;

    public States State
    {
        get { return state; }
        private set
        {
            var prevState = state;
            state = value;

            if (prevState == state)
                return;

            switch (state)
            {
                case States.GoToWork:
                    if (Utils.GetTupleRatio(ability.hp) < 0.5f)
                        State = States.Vacation;
                    else
                    {
                        State = States.Working;
                        myDesk.SetOrigin();
                    }
                    break;
                case States.Working:
                    hpConstant = gm.gameRule.workConsumtionConstant;
                    break;
                case States.OffWork:
                    myDesk.SetOffWork();
                    hpConstant = gm.gameRule.offworkRecoveryConstant;
                    break;
                case States.Vacation:
                    hpConstant = gm.gameRule.vacationRecoveryConstant;
                    break;

                case States.Unassign:
                default:
                    break;
            }
        }
    }

    private void Start()
    {
        hpConstant = 1;
        cumulateWorkload = 0;
        experience = 0;
    }

    private void Update()
    {
        if (State == States.Unassign)
            return;

        switch (state)
        {
            case States.Working:
                UpdateWorking();
                break;
            case States.OffWork:
                UpdateOffWork();
                break;
            case States.Vacation:
                UpdateVacation();
                break;

            case States.Unassign:
            case States.GoToWork:
            default:
                break;
        }
    }

    private void UpdateWorking()
    {
        if (!gm.workTime)
            State = States.OffWork;

        if (eType == WorkType.Player)
            return;

        float deltaTime = gm.deltaTime;
        ability.hp.current -= deltaTime * hpConstant;
        workload.current += deltaTime;
        if (workload.current > workload.amount)
        {
            workload.current = 0f;
            Color color = Color.white;

            WorkDoneType successType = GetDoneType(Random.Range(0f, 1f));
            int workAmount = GetWorkloadAmount();
            switch (successType)
            {
                case WorkDoneType.Fail:
                    color = Color.red;
                    workAmount = 0;
                    break;
                case WorkDoneType.Great:
                    color = Color.yellow;
                    workAmount *= 2;
                    break;
                case WorkDoneType.Success:
                default:
                    break;
            }

            //if (successType != WorkDoneType.Fail)
            {
                bool workOrExp = true;
                switch (eType)
                {
                    case WorkType.Planner:
                        workOrExp = gm.productManager.IncreasePlan(workAmount, GetQuality());
                        break;
                    case WorkType.Developer:
                        workOrExp = gm.productManager.IncreaseDev(workAmount, GetQuality());
                        break;
                    case WorkType.Artist:
                        workOrExp = gm.productManager.IncreaseArt(workAmount, GetQuality());
                        break;
                }
                if (workOrExp)
                    cumulateWorkload += workAmount;
                else
                {
                    experience += workAmount;
                    if (experience >= gm.gameRule.requireExp && ability.CanUpgrade())
                    {
                        experience = 0;
                        ability.Upgrade();
                        SetWorkInit();
                    }    
                }
            }
                
            GameObject floatingUI = gm.DequeueFloatingUI();
            floatingUI.SetActive(true);

            FloatingUI text = floatingUI.GetComponent<FloatingUI>();
            if (successType != WorkDoneType.Fail)
                text.SetText($"+{workAmount}", color);
            else
                text.SetText("실패", color);
            text.SetStartPosition(Camera.main.WorldToScreenPoint(gameObject.transform.position));
        }
    }

    private void UpdateOffWork()
    {
        ability.hp.current += gm.deltaTime * hpConstant;
        if (ability.hp.current > ability.hp.limit)
            ability.hp.current = ability.hp.limit;
    }

    private void UpdateVacation()
    {
        ability.hp.current += gm.deltaTime * hpConstant;
        if (ability.hp.current > ability.hp.limit)
            ability.hp.current = ability.hp.limit;
    }

    public void SetInit(WorkType _eType, EmployeeRating _rating, string _name, EmployeeBaseAblity _ability)
    {
        eType = _eType;
        rating = _rating;
        empName = _name;
        ability = _ability;
        State = States.Unassign;
        gm = GameManager.instance;
        hiredDate = gm.GetToday();
        gameObject.name = _name;

        (float min, float max) salaryRange = Utils.GetIntRange(gm.gameRule.averageSalary[(int)rating], gm.gameRule.salaryRangeRatio);
        salary = (int)NormalDistribution.GetData(salaryRange);
        fakeSalary = (int)(salary * NormalDistribution.GetData(1f, 1.2f));

        SetWorkInit();
        //TestPrint();
    }

    public void GoToWorkTrigger()
    {
        State = States.GoToWork;
    }

    private void SetWorkInit()
    {
        GameRule rule = gm.gameRule;
        // dex
        workload = (0, rule.baseWorkloadAmount + rule.extraWorkloadAmount * (rule.abilityMax - ability.dexterity.current));

        // int
        float sum = rule.successRateMin + (rule.successRateMax - rule.successRateMin) * ((float)ability.intelligence.current / rule.abilityMax);
        greatRate = sum * rule.greatRatio;
        successRate = sum - greatRate;

        // str
        baseWorkloadAmount = ability.strong.current * rule.constantStrongValue + rule.workloadDmgMid;
    }

    private WorkDoneType GetDoneType(float rate)
    {
        if (rate < greatRate)
            return WorkDoneType.Great;
        else if (rate < successRate)
            return WorkDoneType.Success;
        return WorkDoneType.Fail;
    }

    private int GetWorkloadAmount()
    {
        (float min, float max) range = Utils.GetFloatRange(baseWorkloadAmount, gm.gameRule.workloadDmgAmplitude);
        return (int)(NormalDistribution.GetData(range) + 0.5f);
    }

    public void AssignOnDesk(Desk desk)
    {
        myDesk = desk;
        desk.SetOwner(gameObject.GetComponent<Employee>());
        Utils.CopyTransform(gameObject, desk.transform);
        State = (gm.workTime) ? States.Working : States.OffWork;
    }

    public void UnassignOnDesk()
    {
        State = States.Unassign;
        gameObject.transform.position += Vector3.down * 10;
    }

    public void TestPrint()
    {
        Debug.Log($"{eType} {rating} {empName} " +
            $"{hiredDate.GetString()}에 채용\n" +
            $"힘: {ability.strong} " +
            $"민: {ability.dexterity} " +
            $"지: {ability.intelligence} " +
            $"체력: {ability.hp}");
        Debug.Log($"누적 작업량: {cumulateWorkload} 작업 성공률: {successRate * 100:.00} 대성공률: {greatRate * 100:.00}\n" +
            $"경험치: {experience} " +
            $"작업량 범위: {baseWorkloadAmount * (1 - gm.gameRule.workloadDmgAmplitude)} ~ {baseWorkloadAmount * (1 + gm.gameRule.workloadDmgAmplitude)} " +
            $"직원평가: {GetExpectedValue()} 연봉(희망): {salary}");
    }

    public float GetExpectedSuccessRate()
    {
        return successRate + greatRate * 2;
    }

    public float GetExpectedValue()
    {
        // success rate -> accurate, critical rate
        // base workload amount -> damage
        // 60 / workload.amount -> 1 hour / time required to complete the task -> attack speed
        return GetExpectedSuccessRate() * baseWorkloadAmount * (60f / workload.amount);
    }

    private float GetQuality()
    {
        return (ability.strong.current * 0.36f + ability.dexterity.current * 0.24f + ability.intelligence.current * 0.4f) * 0.1f;
    }
}