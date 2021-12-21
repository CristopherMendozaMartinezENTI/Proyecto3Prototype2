using System;
using UnityEngine;

public class RotationCameraFreeze : MonoBehaviour
{
    private CameraController camController;

    private void Start()
    {
        camController = FindObjectOfType<CameraController>();
        if(camController == null)
        {
            Debug.LogError($"{nameof(RotationCameraFreeze)} le falta un {nameof(CameraController)}", this);
            return;
        }

        var gun = FindObjectOfType<TelekinesisController>();

        if (gun != null && camController != null)
        {
            gun.OnRotation.AddListener(OnRotation);
        }
    }

    private void OnRotation(bool rotation)
    {
        camController.LockRotation(rotation);
    }
}
