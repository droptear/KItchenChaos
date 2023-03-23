using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] private Mode _mode;

    private enum Mode
    {
        LookAtCamera,
        LookAtCameraInverted,
        LookAtCameraForward,
        LookAtCameraForwardInverted
    }

    private void LateUpdate()
    {
        switch (_mode)
        {
            case Mode.LookAtCamera:
                transform.LookAt(Camera.main.transform);
                break;
            case Mode.LookAtCameraInverted:
                Vector3 directionFromCamera = transform.position - Camera.main.transform.position;
                transform.LookAt(transform.position + directionFromCamera);
                break;
            case Mode.LookAtCameraForward:
                transform.forward = Camera.main.transform.forward;
                break;
            case Mode.LookAtCameraForwardInverted:
                transform.forward = -Camera.main.transform.forward;
                break;
        }
    }
}
