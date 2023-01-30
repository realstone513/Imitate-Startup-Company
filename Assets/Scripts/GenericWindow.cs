using UnityEngine;
using UnityEngine.EventSystems;
public enum Windows
{
    None = -1,
    Start,
    Keyboard,
    GameOver,
    Difficulty,
}

public class GenericWindow : MonoBehaviour
{
    public GameObject firstSelected;
    public Windows nextWindow = Windows.None;
    public Windows prevWindow = Windows.None;

    public void OnNextWindow()
    {
        WindowManager.m_instance.Open(nextWindow);
    }

    public void OnPrevWindow()
    {
        WindowManager.m_instance.Open(prevWindow);
    }

    protected virtual void Awake()
    {
        Close();
    }

    public void OnFocus()
    {
        EventSystem.current.SetSelectedGameObject(firstSelected);
    }

    protected void Display(bool active)
    {
        gameObject.SetActive(active);
    }

    public virtual void Open()
    {
        Display(true);
        OnFocus();
    }

    public virtual void Close()
    {
        Display(false);
    }
}