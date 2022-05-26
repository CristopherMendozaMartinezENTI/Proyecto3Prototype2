using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Este Script le permite al player Rotarse a si mismo a voluntad 
public class SelfRotate : MonoBehaviour
{
    [SerializeField] private float maxGrabDistance = 3.0f;
    [SerializeField] private Controller _player;
    [SerializeField] private CameraController _camera;
    private Transform newTransformPos;

    private void FixedUpdate()
    {
        Vector3 dir;

        dir = _camera.GetAimingDirection();
        Debug.Log(dir);

        Ray ray = Camera.main.ViewportPointToRay(Vector3.zero * 0.5f);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxGrabDistance))
        {
            Debug.DrawLine(ray.origin, ray.direction * maxGrabDistance, Color.red);
            if (hit.transform != null)
            {
                newTransformPos = hit.transform;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (newTransformPos != null)
                    {
                        SwitchDirection(-dir, _player);
                    }
                    return;
                }

            }
        }
    }

    void SwitchDirection(Vector3 _newUpDirection, Controller _controller)
    {
        float _angleThreshold = 0.001f;
        float _angleBetweenUpDirections = Vector3.Angle(_newUpDirection, _controller.transform.up);
        if (_angleBetweenUpDirections < _angleThreshold)
            return;

        //audioSource.Play();

        Transform _transform = _controller.transform;
        Quaternion _rotationDifference = Quaternion.FromToRotation(_transform.up, _newUpDirection);
        _transform.rotation = _rotationDifference * _transform.rotation;
    }
}
