using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public struct Date
{
    public int year;
    public int month;
    public int week;

    public void SetInit(int year, int month, int week)
    {
        this.year = year;
        this.month = month;
        this.week = week;
    }

    public string GetString()
    {
        return $"{year}�� {month}�� {week}��";
    }
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public TextMeshProUGUI ymwText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI retirementText;
    public Slider timerSlider;
    public Button[] buttons;
    public TextMeshProUGUI playModeText;

    public GameObject floatingTextPrefab;
    public Transform floatingUITransform;
    private int queueSize = 20;
    private Queue<GameObject> floatingUIQueue;

    private Date date;
    private (float hour, float minute) timer = (0f, 0f);

    private int timeScale = 0;

    private readonly (float hour, float minute) goToWorkTime = (10, 00);
    private readonly (float hour, float minute) offWorkTime = (19, 00);
    private bool onSkip = false;
    public bool workTime = false;
    private bool beforeWorkTime = false;
    public float deltaTime = 0f;

    public LayerMask clickableLayer;
    RaycastHit hit;
    private Desk currentDesk;
    public GameRule gameRule;
    public List<Desk> desks;
    public List<GameObject> chairs;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        date.SetInit(1, 1, 1);
        SetYmdText();
        timerText.text = "00:00";

        floatingUIQueue = new Queue<GameObject>(queueSize);
        for (int i = 0; i < queueSize; i++)
            floatingUIQueue.Enqueue(Instantiate(floatingTextPrefab, floatingUITransform));
    }

    private void Start()
    {
        currentDesk = desks[0];
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SetTimeScale(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            SetTimeScale(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            SetTimeScale(2);
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            SetTimeScale(3);

        if (Input.GetKeyDown(KeyCode.P))
        {
            MoveToOffice();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            MoveToMeeting();
        }

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, clickableLayer))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    currentDesk = hit.collider.gameObject.GetComponent<Desk>();
                    // Debug.Log(hit.collider.name);
                    WindowManager.instance.Open(Windows.EmptyWorkspace);
                }
            }
        }

        {
            deltaTime = (onSkip ? gameRule.constantSkipSpeed : gameRule.constantSpeed * timeScale) * Time.deltaTime;

            if (timeScale == 0)
                return;

            timer.minute += deltaTime;
            timerSlider.value += deltaTime;
            if (timer.minute >= 60f)
            {
                timer.minute = 0f;
                timer.hour++;
            }
            timerText.text = $"{timer.hour:00.}:{timer.minute:00.}";

            if (timer.hour >= 24f)
            {
                timer = (0f, 0f);
                timerSlider.value = 0f;
                date.week++;
                if (date.week > 4)
                {
                    date.month++;
                    date.week = 1;
                }
                if (date.month > 12)
                {
                    date.year++;
                    date.month = 1;
                }
                SetYmdText();
            }

            beforeWorkTime = workTime;
            workTime = !(AfterOffWorkTime() || BeforeGoToWorkTime());
            if (!beforeWorkTime && workTime)
                EmployeeManager.instance.GotoWorkTrigger();

            if (timeScale == 3)
                SetSkipMode(!workTime);
        }
    }

    private void SetYmdText()
    {
        ymwText.text = date.GetString();
        retirementText.text = $"{gameRule.endYear - date.year}�� �ڿ� ����";
    }

    public void SetTimeScale(int value)
    {
        timeScale = value;
        buttons[value].Select();
        if (value != 0)
            playModeText.text = value == 3 && onSkip ? "Skip" : $"x{value}";
        else
            playModeText.text = $"Stop";

        if (!onSkip && timeScale == 3 && !workTime)
            SetSkipMode(true);
        else onSkip = false;
    }

    public void SetSkipMode(bool value)
    {
        if (onSkip == value)
            return;

        onSkip = value;
        if (onSkip)
            playModeText.text = "Skip";
        else
            playModeText.text = $"x{timeScale}";
    }

    public void MoveToMeeting()
    {
        int size = desks.Count;
        for (int i = 0; i < size; i++)
        {
            Employee employee = desks[i].GetOwner();
            if (employee == null)
            {
                continue;
            }
            Utils.CopyTransform(employee.gameObject, chairs[i].transform);
        }
    }

    public void MoveToOffice()
    {
        int size = desks.Count;
        for (int i = 0; i < size; i++)
        {
            if (desks[i].GetOwner() != null)
                desks[i].SetOrigin();
        }
    }

    private bool BeforeGoToWorkTime()
    {
        return timer.hour < goToWorkTime.hour ||
            (timer.hour == goToWorkTime.hour &&
            timer.minute <= goToWorkTime.minute);
    }

    private bool AfterOffWorkTime()
    {
        return timer.hour > offWorkTime.hour ||
            (timer.hour == offWorkTime.hour &&
            timer.minute >= offWorkTime.minute);
    }

    public Date GetToday()
    {
        return date;
    }

    public Desk GetCurrentDesk()
    {
        return currentDesk;
    }

    public GameObject DequeueFloatingUI()
    {
        return floatingUIQueue.Dequeue();
    }

    public void EnqueueFloatingUI(GameObject ui)
    {
        floatingUIQueue.Enqueue(ui);
    }
}