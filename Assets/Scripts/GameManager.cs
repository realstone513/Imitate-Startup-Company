namespace Realstone
{
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;

    public struct Date
    {
        public int year;
        public int month;
        public int day;

        public void SetInit(int year, int month, int day)
        {
            this.year = year;
            this.month = month;
            this.day = day;
        }

        public string GetString()
        {
            return $"{year}년 {month}월 {day}일";
        }
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        public EmployeeManager employeeManager;
        public ProductManager productManager;

        public TextMeshProUGUI ymwText;
        public TextMeshProUGUI timerText;
        public TextMeshProUGUI retirementText;
        public Slider timerSlider;
        public Button[] buttons;
        public TextMeshProUGUI playModeText;
        public TextMeshProUGUI moneyText;

        public GameObject floatingTextPrefab;
        public Transform floatingUITransform;
        private readonly int queueSize = 20;
        private Queue<GameObject> floatingUIQueue;

        private Date date;
        private (float hour, float minute) timer = (0f, 0f);

        private int timeScale = 0;

        private readonly (float hour, float minute) goToWorkTime = (10, 00);
        private readonly (float hour, float minute) offWorkTime = (19, 00);
        private bool onSkip = false;
        private bool beforeWorkTime = false;
        public bool workTime = false;
        public float deltaTime = 0f;

        public LayerMask clickableLayer;
        private RaycastHit hit;
        private Desk currentDesk;
        public List<Desk> desks;
        public GameRule gameRule;
        public int money;
        public bool inputFieldMode;

        public Dictionary<string, int> financeLossDictionary = new();
        public Dictionary<string, int> financeProfitDictionary = new();

        public bool popupMode;

        //public List<GameObject> chairs;
        //public bool isMeeting;
        //private MainCameraManager mcm;

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
            //isMeeting = false;

            floatingUIQueue = new Queue<GameObject>(queueSize);
            for (int i = 0; i < queueSize; i++)
                floatingUIQueue.Enqueue(Instantiate(floatingTextPrefab, floatingUITransform));
            //mcm = Camera.main.GetComponent<MainCameraManager>();
            popupMode = false;
        }

        private void Start()
        {
            currentDesk = desks[0];
            money = gameRule.seedMoney;
            inputFieldMode = false;
        }

        private void Update()
        {
            if (!inputFieldMode)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                    SetTimeScale(0);
                else if (Input.GetKeyDown(KeyCode.Alpha2))
                    SetTimeScale(1);
                else if (Input.GetKeyDown(KeyCode.Alpha3))
                    SetTimeScale(2);
                else if (Input.GetKeyDown(KeyCode.Alpha4))
                    SetTimeScale(3);
            }

            //if (Input.GetKeyDown(KeyCode.P))
            //{
            //    MoveToOffice();
            //}
            //if (Input.GetKeyDown(KeyCode.I))
            //{
            //    MoveToMeeting();
            //}

            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, clickableLayer))
                {
                    if (Input.GetMouseButtonDown(0) && !popupMode)
                    {
                        currentDesk = hit.collider.gameObject.GetComponent<Desk>();
                        if (currentDesk.GetOwner() == null)
                            WindowManager.instance.Open(Windows.EmptyWorkspace);
                        else
                        {
                            WindowManager.instance.Open(Windows.SelectEmployee);
                        }
                    }
                }
            }

            {
                //deltaTime = Time.deltaTime;
                //if (!isMeeting)

                deltaTime = Time.deltaTime * (onSkip ? gameRule.constantSkipSpeed : gameRule.constantSpeed * timeScale);

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
                    date.day++;
                    if (date.day > 4)
                    {
                        date.month++;
                        date.day = 1;
                        CalculateMonthIncome();
                        ClearSalarys();
                        ClearProductIncome();
                        SetSalarys();
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
                    employeeManager.GotoWorkTrigger();

                if (timeScale == 3)
                    SetSkipMode(!workTime);
            }
        }

        private void SetYmdText()
        {
            ymwText.text = date.GetString();
            retirementText.text = $"{gameRule.endYear - date.year}년 뒤에 종료";
        }

        public void SetTimeScale(int value)
        {
            timeScale = value;
            buttons[value].Select();
            if (value != 0)
                playModeText.text = value == 3 && onSkip ? "스킵" : $"x{value}";
            else
                playModeText.text = $"정지";

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
                playModeText.text = "스킵";
            else
                playModeText.text = $"x{timeScale}";
        }

        /*public void MoveToMeeting()
        {
            isMeeting = true;
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
            isMeeting = false;
            int size = desks.Count;
            for (int i = 0; i < size; i++)
            {
                if (desks[i].GetOwner() != null)
                    desks[i].SetOrigin();
            }
        }*/

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

        //public (float hour, float minute) GetGoToWorkTime()
        //{
        //    return goToWorkTime;
        //}

        //public (float hour, float minute) GetOffWorkTime()
        //{
        //    return offWorkTime;
        //}

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

        public void TranslateGameMoney(int amount)
        {
            money += amount;
            moneyText.text = $"{money}";

            GameObject floatingUI = DequeueFloatingUI();
            floatingUI.SetActive(true);

            FloatingUI text = floatingUI.GetComponent<FloatingUI>();
            Color color = amount < 0 ? Color.red : Color.green;
            text.SetText($"{amount}", color);
            Vector3 mod = new(-15f, 0f, 0f);
            text.SetStartPosition(moneyText.transform.position + mod);
        }

        public int CalculateMonthSalary(int yearSalary)
        {
            // 1 month = 4 day
            int numberOfDaysLeft = 4 - date.day + 1;
            int monthSalary = yearSalary / 12;
            return (monthSalary * numberOfDaysLeft / 4);
        }

        private void SetSalarys()
        {
            var unassign = employeeManager.GetUnassign();
            var assign = employeeManager.GetAssign();
            foreach (var emp in unassign)
            {
                Employee employee = emp.GetComponent<Employee>();
                financeLossDictionary[$"{employee.empName} 월급"] = CalculateMonthSalary(employee.salary);
            }
            foreach (var emp in assign)
            {
                Employee employee = emp.GetComponent<Employee>();
                if (employee.eType == WorkType.Player)
                    continue;
                financeLossDictionary[$"{employee.empName} 월급"] = CalculateMonthSalary(employee.salary);
            }
        }

        private void CalculateMonthIncome()
        {
            int lossSum = 0;
            foreach (var elem in financeLossDictionary)
            {
                lossSum += elem.Value;
            }
            int profitSum = 0;
            foreach (var elem in financeProfitDictionary)
            {
                profitSum += elem.Value;
            }
            int total = profitSum - lossSum;
            TranslateGameMoney(total);
        }

        private void ClearSalarys()
        {
            var unassign = employeeManager.GetUnassign();
            var assign = employeeManager.GetAssign();
            foreach (var emp in unassign)
            {
                Employee employee = emp.GetComponent<Employee>();
                financeLossDictionary.Remove($"{employee.empName} 월급");
            }
            foreach (var emp in assign)
            {
                Employee employee = emp.GetComponent<Employee>();
                if (employee.eType == WorkType.Player)
                    continue;
                financeLossDictionary.Remove($"{employee.empName} 월급");
            }
        }

        private void ClearProductIncome()
        {
            var products = productManager.products;
            foreach (var product in products)
            {
                product.ClearMonthlyIncome();
            }
        }

        private void ClearFinance()
        {
            financeLossDictionary.Clear();
            financeProfitDictionary.Clear();
        }
    }
}