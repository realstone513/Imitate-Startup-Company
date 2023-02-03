using System.Collections;
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

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.P))
    //    {
    //        CameraMove(CameraMode.office);
    //    }
    //    else if (Input.GetKeyDown(KeyCode.O))
    //    {
    //        CameraMove(CameraMode.officeZoom);
    //    }
    //    else if (Input.GetKeyDown(KeyCode.I))
    //    {
    //        CameraMove(CameraMode.meeting);
    //    }
    //}

    private IEnumerator MeetingIntro()
    {
        yield return new WaitForSeconds(2.0f);

        float timer = 0f;
        while (timer < 2.5f)
        {
            gameObject.transform.Translate(0, 0, Time.deltaTime * 2);
            timer += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(2.0f);
        timer = 0f;
        while (timer < 1.75f)
        {
            gameObject.transform.Translate(0, 0, -2 * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }
    }

    public void CameraMove(CameraMode mode)
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