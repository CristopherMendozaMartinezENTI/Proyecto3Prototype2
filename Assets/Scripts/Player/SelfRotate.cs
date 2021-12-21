using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SelfRotate : MonoBehaviour
{
    [SerializeField] private float maxGrabDistance = 3.0f;
    [SerializeField] private Controller player;
    [SerializeField] private GameObject camera;
    private Transform newTransformPos;

    private void FixedUpdate()
    {
        Vector3 dir;
        float x, y, z;

        dir = camera.transform.forward.normalized;
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
                    // Reset the rigidbody to how it was before we grabbed it
                    if (newTransformPos != null)
                    {
                        SwitchDirection(-dir, player);
                    }
                    return;
                }

            }
        }
    }

    void SwitchDirection(Vector3 _newUpDirection, Controller _controller)
    {
        float _angleThreshold = 0.001f;

        //Calculate angle;
        float _angleBetweenUpDirections = Vector3.Angle(_newUpDirection, _controller.transform.up);

        //If angle between new direction and current rigidbody rotation is too small, return;
        if (_angleBetweenUpDirections < _angleThreshold)
            return;

        //Play audio cue;
        //audioSource.Play();

        Transform _transform = _controller.transform;

        //Rotate gameobject;
        Quaternion _rotationDifference = Quaternion.FromToRotation(_transform.up, _newUpDirection);
        _transform.rotation = _rotationDifference * _transform.rotation;
    }
}
