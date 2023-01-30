using UnityEngine;

public struct EmployeeBaseAblity
{
    // 창의력, 성실성, 주도성, 체력
    public (int current, int limit) creativity;
    public (int current, int limit) conscientiousness;
    public (int current, int limit) scrupulosity;
    public (int current, int limit) hp;

    public EmployeeBaseAblity(
        (int min, int max) range, int cre, int con, int scr, int hp)
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
    private States state;
    public EmployeeType eType;
    public EmployeeRating rating;
    public Date hiredDate;

    private void Update()
    {
        if (state == States.None)
            return;


    }

    public void SetInit(EmployeeType _eType, EmployeeRating _rating, string _name, EmployeeBaseAblity _ability)
    {
        eType = _eType;
        rating = _rating;
        empName = _name;
        ability = _ability;
        state = States.None;
        hiredDate = GameManager.instance.GetToday();

        TestPrint();
    }

    public void PlaceOnDesk(Vector3 pos)
    {
        state = States.Working;
        gameObject.transform.position = pos;
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