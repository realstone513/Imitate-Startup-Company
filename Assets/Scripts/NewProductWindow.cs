using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum WorkLevel
{
    minimum,
    fast,
    quality,
    best,
}

public class NewProductWindow : GenericWindow
{
    public TMP_InputField projectNameInput;
    public Button submitButton;
    private WorkLevel planLevel;
    private WorkLevel devLevel;
    private WorkLevel artLevel;

    private void OnEnable()
    {
        submitButton.interactable = false;
    }

    public void EditName()
    {
        submitButton.interactable = projectNameInput.text != string.Empty;
    }

    public void GetPlanLevel(Int32 value)
    {
        planLevel = (WorkLevel)value;
    }

    public void GetDevLevel(Int32 value)
    {
        devLevel = (WorkLevel)value;
    }

    public void GetArtLevel(Int32 value)
    {
        artLevel = (WorkLevel)value;
    }

    public void Submit()
    {
        int plan = (int)planLevel;
        int dev = (int)devLevel;
        int art = (int)artLevel;

        GameRule rule = GameManager.instance.gameRule;
        ProductManager.instance.NewProduct(
            projectNameInput.text, rule.planAmountChart[plan], rule.planAmountChart[dev], rule.planAmountChart[art]);
        Close();
    }
}