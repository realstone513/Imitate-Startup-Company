using UnityEngine;

public enum Windows
{
    None = -1,
    EmptyWorkspace,
}

public class WindowManager : MonoBehaviour
{
    public static WindowManager instance;

    public GenericWindow[] windows;
    public int currentWndId;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        AllClose();
        currentWndId = -1;
        //Open(defaultWndId);
    }

    public GenericWindow GetWindow(int id)
    {
        return windows[id];
    }

    private void ToggleWindow(int id)
    {
        for (int i = 0; i < windows.Length; i++)
        {
            if (i == id)
                windows[i].Open();
            else if (windows[i].gameObject.activeSelf)
                windows[i].Close();
        }
    }

    public GenericWindow Open(int id)
    {
        if (id < 0 || windows.Length <= id)
            return null;

        currentWndId = id;
        ToggleWindow(currentWndId);
        return GetWindow(currentWndId);
    }

    public GenericWindow Open(Windows id)
    {
        return Open((int)id);
    }

    public void AllClose()
    {
        foreach (var window in windows)
            window.Close();

        currentWndId = -1;
    }
}