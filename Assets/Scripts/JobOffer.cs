using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JobOffer : GenericWindow
{
    public TextMeshProUGUI recruitCostText;
    public List<GameObject> employeeInfoList = new();
    private List<GameObject> employeeList = new();
    private EmployeeRating rating = EmployeeRating.Beginner;
    private WorkType workType = WorkType.Planner;
    private int cost = 10;
    private int offerIndex = -1;
    public GameObject offerWindow;
    public TextMeshProUGUI firstLog;
    public GameObject logPrefab;
    public Transform logTransform;
    private bool offerSuccess;
    public TextMeshProUGUI salaryText;
    public Slider salarySlider;
    public TextMeshProUGUI bonusText;
    public Slider bonusSlider;

    private void OnEnable()
    {
        InfoActive(false);
        offerWindow.SetActive(false);
        offerSuccess = false;
    }

    public void ChangeCostText(Int32 num)
    {
        GameRule rule = GameManager.instance.gameRule;
        switch (num)
        {
            case 0:
                cost = rule.jobOfferCostBeginner;
                rating = EmployeeRating.Beginner;
                break;
            case 1:
                cost = rule.jobOfferCostIntermediate;
                rating = EmployeeRating.Intermediate;
                break; 
            case 2:
                cost = rule.jobOfferCostExpert;
                rating = EmployeeRating.Expert;
                break;
        }
        recruitCostText.text = $"{cost}";
    }

    public void ChangeWorkType(Int32 num)
    {
        switch (num)
        {
            case 0:
                workType = WorkType.Planner;
                break;
            case 1:
                workType = WorkType.Developer;
                break;
            case 2:
                workType = WorkType.Artist;
                break;
        }
    }

    public void Offer()
    {
        if (GameManager.instance.money <= cost)
            return;

        CreateNewEmployees();
        InfoActive(true);
        offerWindow.SetActive(false);
    }

    public void SelectInfo(int idx)
    {
        offerIndex = idx;
        offerWindow.SetActive(true);
        Debug.Log(offerIndex);
        Employee thisEmployee = employeeList[idx].GetComponent<Employee>();
        firstLog.text = $"{thisEmployee.empName}은(는)\n{thisEmployee.salary}만원을 원합니다.";
        InfoActive(false);
    }

    private void CreateNewEmployees()
    {
        ClearList();

        GameManager.instance.TranslateGameMoney(-cost);
        for (int i = 0; i < 3; i++)
        {
            employeeList.Add(EmployeeManager.instance.CreateNewEmployee(rating, workType));
            employeeInfoList[i].GetComponent<EmployeeInfo>().SetInfo(employeeList[i].GetComponent<Employee>());
        }

    }
    
    private void ClearList()
    {
        int size = employeeList.Count;
        for (int i = 0; i < size; i++)
            Destroy(employeeList[i]);

        employeeList.Clear();
    }

    private void InfoActive(bool value)
    {
        foreach (var info in employeeInfoList)
            info.SetActive(value);
    }

    private void OnDisable()
    {
        ClearList();
        offerIndex = -1;
    }
}