using UnityEngine;

public struct EmployeeBaseAblity
{
    // 창의력, 성실성, 주도성, 체력
    public (int current, int limit) creativity;
    public (int current, int limit) conscientiousness;
    public (int current, int limit) scrupulosity;
    public (float current, float limit) hp;

    public EmployeeBaseAblity(
        (int min, int max) range, int cre, int con, int scr, float hp)
    {
        creativity = (Random.Range(range.min, cre + 1), cre);
        conscientiousness = (Random.Range(range.min, con + 1), con);
        scrupulosity = (Random.Range(range.min, scr + 1), scr);
        this.hp = (hp, hp);
    }
}

public enum States
{
    None = -1,
    GoToWork,
    Working,
    OffWork,
    Vacation,
    //Education
}

public enum EmployeeType
{
    None = -1,
    Planner,
    Developer,
    Artist,
    // Player,
}

public class Employee : MonoBehaviour
{
    public string empName;
    private EmployeeBaseAblity ability;
    private (float current, float amount) workload;
    public States state;
    public EmployeeType eType;
    public EmployeeRating rating;
    public Date hiredDate;
    private int constantChange;
    private int experiment;

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
                        transform.position += Vector3.up * 10;
                    }
                    break;
                case States.Working:
                    constantChange = 5;
                    break;
                case States.OffWork:
                    transform.position += Vector3.down * 10;
                    constantChange = 2;
                    break;
                case States.Vacation:
                    constantChange = 3;
                    break;
                /*case States.Education:
                    break;*/

                case States.None:
                default:
                    break;
            }
        }
    }

    private void Start()
    {
        SetWorkload();
        constantChange = 1;
        experiment = 0;
    }

    private void Update()
    {
        if (State == States.None)
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
            /*case States.Education:
                UpdateEducation();
                break;*/

            case States.None:
            case States.GoToWork:
            default:
                break;
        }
    }

    private void UpdateWorking()
    {
        float deltaTime = GameManager.instance.deltaTime;
        ability.hp.current -= deltaTime * constantChange;
        workload.current += deltaTime;
        if (workload.current > workload.amount)
        {
            workload.current = 0f;
            switch (eType)
            {
                case EmployeeType.Planner:
                    if (!ProductManager.instance.IncreasePlan())
                        experiment++;
                    break;
                case EmployeeType.Developer:
                    if (!ProductManager.instance.IncreaseDev())
                        experiment++;
                    break;
                case EmployeeType.Artist:
                    if (!ProductManager.instance.IncreaseArt())
                        experiment++;
                    break;
            }
            GameObject dmgUI = GameManager.instance.DequeueDmgUI();
            dmgUI.SetActive(true);

            FloatingUI text = dmgUI.GetComponent<FloatingUI>();
            text.SetText("+1");
            text.SetStartPosition(Camera.main.WorldToScreenPoint(gameObject.transform.position));

            Debug.Log($"{empName} 작업 끝");
        }

        if (!GameManager.instance.workTime)
            State = States.OffWork;
    }

    private void UpdateOffWork()
    {
        ability.hp.current += GameManager.instance.deltaTime * constantChange;
        if (ability.hp.current > ability.hp.limit)
            ability.hp.current = ability.hp.limit;
    }

    private void UpdateVacation()
    {
        ability.hp.current += GameManager.instance.deltaTime * constantChange;
        if (ability.hp.current > ability.hp.limit)
            ability.hp.current = ability.hp.limit;
    }

    /*private void UpdateEducation()
    {
        float deltaTime = GameManager.instance.deltaTime;
        ability.hp.current -= deltaTime * 3;
        workload.current += deltaTime;
        if (workload.current > workload.amount)
        {
            workload.current = 0f;
            Debug.Log($"{empName} 교육 끝");
        }

        if (!GameManager.instance.workTime)
            State = States.OffWork;
    }*/

    public void SetInit(EmployeeType _eType, EmployeeRating _rating, string _name, EmployeeBaseAblity _ability)
    {
        eType = _eType;
        rating = _rating;
        empName = _name;
        ability = _ability;
        State = States.None;
        hiredDate = GameManager.instance.GetToday();
        gameObject.name = _name;
        TestPrint();
    }

    public void GoToWorkTrigger()
    {
        State = States.GoToWork;
    }

    private void SetWorkload()
    {
        GameRule rule = GameManager.instance.gameRule;
        workload = (0, rule.baseWorkloadAmount + rule.ratioWorkloadAmount * (rule.abilityMax - ability.conscientiousness.current));
    }

    public void AssignOnDesk(Vector3 pos)
    {
        gameObject.transform.position = pos;
        State = (GameManager.instance.workTime) ? States.Working : States.OffWork;
        Debug.Log($"{empName} {State}");
    }

    public void UnassignOnDesk()
    {
        State = States.None;
        gameObject.transform.position += Vector3.down * 10;
    }

    public void TestPrint()
    {
        Debug.Log($"{eType} {rating} {empName} " +
            $"{hiredDate.GetString()}에 채용\n" +
            $"창의성: {ability.creativity} " +
            $"성실성: {ability.conscientiousness} " +
            $"주도성: {ability.scrupulosity} " +
            $"체력: {ability.hp}");
        // Debug.Log($"");
    }
}