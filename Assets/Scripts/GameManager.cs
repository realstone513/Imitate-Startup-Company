using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public TextMeshProUGUI ymwText;
    public TextMeshProUGUI timerText;
    private int year = 1;
    private int month = 1;
    private int week = 1;
    private (float hour, float minute) timer = (0f, 0f);

    private int timeScale = 0;
    public int constantSpeed = 5;
    public int constantSkipSpeed = 100;

    private readonly (float hour, float minute) goToWorkTime = (10, 00);
    private readonly (float hour, float minute) offWorkTime = (19, 00);
    private bool onSkip = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // DontDestroyOnLoad(this.gameObject);
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
            timer.minute += (additionalSpeed *  Time.deltaTime);
            if (timer.minute >= 60f)
            {
                timer.minute = 0f;
                timer.hour++;
            }
            timerText.text = $"{timer.hour:00.}:{timer.minute:00.}";

            if (timer.hour >= 24f)
            {
                Debug.Log("Next Week");
                timer = (0f, 0f);
                week++;
                if (week > 4)
                    month++;
                if (month > 12)
                    year++;
                SetYmdText();
            }
            if (onSkip && (!AfterOffWorkTime() && !BeforeGoToWorkTime()))
            {
                Debug.Log("Stop skip");
                onSkip = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            timeScale = 0;
            Debug.Log("Timer stop");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            timeScale = 1;
            Debug.Log("Timer x1");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            timeScale = 2;
            Debug.Log("Timer x2");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            timeScale = 3;
            Debug.Log("Timer x3");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (AfterOffWorkTime() || BeforeGoToWorkTime())
            {
                onSkip = true;
                Debug.Log("Start Skip");
            }
            else Debug.Log("Fail Skip");
        }
    }

    private void SetYmdText()
    {
        ymwText.text = $"{year}년차 {month}월 {week}주차";
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