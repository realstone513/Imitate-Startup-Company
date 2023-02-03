using UnityEngine;

public enum CameraMode
{
    office,
    officeZoom,
    meetingActive,
}

public class MainCameraManager : MonoBehaviour
{
    public Transform officeDefault;
    public Transform officeZoom;

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
        //else if (Input.GetKeyDown(KeyCode.O))
        //{
        //    CameraMove(CameraMode.meeting);
        //}
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
    }
}