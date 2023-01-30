using UnityEngine;

public class WindowManager : MonoBehaviour
{
    public static WindowManager m_instance;

    public GenericWindow[] windows;
    public int currentWndId;
    public int defaultWndId;

    private void Awake()
    {
        m_instance = this;
    }

    private void Start()
    {
        Open(defaultWndId);
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
}