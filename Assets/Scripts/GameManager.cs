using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        return $"{year}년 {month}월 {week}주";
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

    private Date date;
    private (float hour, float minute) timer = (0f, 0f);

    private int timeScale = 0;
    public int constantSpeed = 5;
    public int constantSkipSpeed = 100;
    private int endYear = 10;

    private readonly (float hour, float minute) goToWorkTime = (10, 00);
    private readonly (float hour, float minute) offWorkTime = (19, 00);
    private bool onSkip = false;

    public LayerMask clickableLayer;
    RaycastHit hit;
    private GameObject currentDesk;

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
    }

    private void Update()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, clickableLayer))
        {
            if (Input.GetMouseButtonDown(0))
            {
                currentDesk = hit.collider.gameObject;
                WindowManager.instance.Open(Windows.EmptyWorkspace);
            }
        }

        if (timeScale != 0)
        {
            int additionalSpeed = onSkip ? constantSkipSpeed : constantSpeed * timeScale;
            float adder = (additionalSpeed * Time.deltaTime);
            timer.minute += adder;
            timerSlider.value += adder;
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

            if (!onSkip && timeScale == 3 && (AfterOffWorkTime() || BeforeGoToWorkTime()))
                SetSkipMode(true);
            else if (onSkip && !(AfterOffWorkTime() || BeforeGoToWorkTime()))
                SetSkipMode(false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
            SetTimeScale(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            SetTimeScale(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            SetTimeScale(2);
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            SetTimeScale(3);
    }

    private void SetYmdText()
    {
        ymwText.text = date.GetString();
        retirementText.text = $"{endYear - date.year}년 뒤에 종료";
    }

    public void SetTimeScale(int value)
    {
        timeScale = value;
        buttons[value].Select();
        if (value != 0)
            playModeText.text = value == 3 && onSkip ? "Skip" : $"x{value}";
        else
            playModeText.text = $"Stop";
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

    public (float hour, float minute) GetTimer()
    {
        return timer;
    }

    public (float hour, float minute) GetGoToWorkTime()
    {
        return goToWorkTime;
    }

    public (float hour, float minute) GetOffWorkTime()
    {
        return offWorkTime;
    }

    public Date GetToday()
    {
        return date;
    }

    public GameObject GetCurrentDesk()
    {
        return currentDesk;
    }
}