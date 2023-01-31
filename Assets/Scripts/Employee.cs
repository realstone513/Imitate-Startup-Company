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
    Education
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
    private (float current, float duration) timer;
    public States state;
    public EmployeeType eType;
    public EmployeeRating rating;
    public Date hiredDate;
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
                    timer = (0f, 100f);
                    break;
                case States.OffWork:
                    transform.position += Vector3.down * 10;
                    break;
                case States.Vacation:
                    break;
                case States.Education:
                    break;

                case States.None:
                default:
                    break;
            }
        }
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
            case States.Education:
                UpdateEducation();
                break;

            case States.None:
            case States.GoToWork:
            default:
                break;
        }
    }

    private void UpdateWorking()
    {
        float deltaTime = GameManager.instance.deltaTime;
        ability.hp.current -= deltaTime * 3;
        timer.current += deltaTime;
        if (timer.current > timer.duration)
        {
            timer.current = 0f;
            Debug.Log($"{empName} 작업 끝");
        }

        if (!GameManager.instance.workTime)
            State = States.OffWork;
    }

    private void UpdateOffWork()
    {
        ability.hp.current += GameManager.instance.deltaTime;
        if (ability.hp.current > ability.hp.limit)
            ability.hp.current = ability.hp.limit;
        if (GameManager.instance.workTime)
            State = States.GoToWork;
    }

    private void UpdateVacation()
    {
        ability.hp.current += GameManager.instance.deltaTime;
        if (ability.hp.current > ability.hp.limit)
            ability.hp.current = ability.hp.limit;
        if (GameManager.instance.workTime)
            State = States.GoToWork;
    }

    private void UpdateEducation()
    {
        float deltaTime = GameManager.instance.deltaTime;
        ability.hp.current -= deltaTime * 3;
        timer.current += deltaTime;
        if (timer.current > timer.duration)
        {
            timer.current = 0f;
            Debug.Log($"{empName} 교육 끝");
        }

        if (!GameManager.instance.workTime)
            State = States.OffWork;
    }

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
    }
}