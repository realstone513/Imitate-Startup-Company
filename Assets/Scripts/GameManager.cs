using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public TextMeshProUGUI ymwText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI retirementText;
    public Slider timerSlider;
    public Button[] buttons;

    private int year = 1;
    private int month = 1;
    private int week = 1;
    private (float hour, float minute) timer = (0f, 0f);

    private int timeScale = 0;
    public int constantSpeed = 5;
    public int constantSkipSpeed = 100;
    private int endYear = 10;

    private readonly (float hour, float minute) goToWorkTime = (10, 00);
    private readonly (float hour, float minute) offWorkTime = (19, 00);
    private bool onSkip = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        SetYmdText();
        timerText.text = "00:00";
    }

    private void Update()
    {
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
                week++;
                if (week > 4)
                {
                    month++;
                    week = 1;
                }
                if (month > 12)
                {
                    year++;
                    month = 1;
                }
                SetYmdText();
            }

            if (!onSkip && timeScale == 3 && (AfterOffWorkTime() || BeforeGoToWorkTime()))
            {
                SetSkipMode(true);
            }

            if (onSkip && !(AfterOffWorkTime() || BeforeGoToWorkTime()))
            {
                SetSkipMode(false);
            }
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
        ymwText.text = $"{year}년 {month}월 {week}주";
        retirementText.text = $"{endYear - year}년 뒤에 종료";
    }

    public void SetTimeScale(int value)
    {
        timeScale = value;
        buttons[value].Select();
    }

    public void SetSkipMode(bool value)
    {
        onSkip = value;
        Debug.Log($"Skip {(onSkip ? "Start" : "Stop")}");
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
}