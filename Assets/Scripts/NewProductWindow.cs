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
        projectNameInput.text = "";
        projectNameInput.text = string.Empty;
        GameManager.instance.inputFieldMode = false;
    }

    public void EditName()
    {
        GameManager.instance.inputFieldMode = true;
        submitButton.interactable = projectNameInput.text != string.Empty;
        LimitTextSize(10);
    }

    public void EndEdit()
    {
        GameManager.instance.inputFieldMode = false;
        LimitTextSize(10);
    }

    private void LimitTextSize(int size)
    {
        if (projectNameInput.text.Length > size)
            projectNameInput.text = projectNameInput.text[..size];
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

        GameManager gm = GameManager.instance;
        gm.productManager.NewProduct(
            projectNameInput.text, gm.gameRule.planAmountChart[plan], gm.gameRule.planAmountChart[dev], gm.gameRule.planAmountChart[art]);
        Close();
    }
}