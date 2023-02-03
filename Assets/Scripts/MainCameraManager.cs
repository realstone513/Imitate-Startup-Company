using System.Collections;
using System.Threading;
using UnityEngine;

public enum CameraMode
{
    office,
    officeZoom,
    meeting,
}

public class MainCameraManager : MonoBehaviour
{
    public Transform officeDefault;
    public Transform officeZoom;
    public Transform meetingDefault;

    CameraMode cameraMode;

    private void Start()
    {
        cameraMode = CameraMode.office;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            CameraMove(CameraMode.office);
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            CameraMove(CameraMode.officeZoom);
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            CameraMove(CameraMode.meeting);
        }
    }



    public IEnumerator MeetingIntro()
    {
        yield return new WaitForSeconds(2.0f);

        float timer = 0f;
        while (timer < 5f)
        {
            gameObject.transform.Translate(0, 0, Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }
    }

    private void CameraMove(CameraMode mode)
    {
        cameraMode = mode;
        if (cameraMode == CameraMode.office)
        {
            Utils.CopyTransform(gameObject, officeDefault);
        }
        else if (cameraMode == CameraMode.officeZoom)
        {
            Utils.CopyTransform(gameObject, officeZoom);
        }
        else if (cameraMode == CameraMode.meeting)
        {
            Utils.CopyTransform(gameObject, meetingDefault);
            StartCoroutine(MeetingIntro());
        }
    }
}