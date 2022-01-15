using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ZoomType { ZoomIn, DollyZoomOut }

public class TriggerZoom : MonoBehaviour
{
    [SerializeField] private ZoomType zoomType;
    [SerializeField] private float zoomMultiplier = 2.0f;
    [SerializeField] private float zoomDuration = 0.5f;
    private float targetFov;
    private bool onTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            switch (zoomType)
            {
                case ZoomType.ZoomIn:
                    onTrigger = true;
                    return;
                case ZoomType.DollyZoomOut:
                    onTrigger = false;
                    return;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            switch (zoomType)
            {
                case ZoomType.ZoomIn:
                    onTrigger = false;
                    return;
                case ZoomType.DollyZoomOut:
                    onTrigger = true;
                    return;
            }
        }
    }

    private void Start()
    {
        switch (zoomType)
        {
            case ZoomType.ZoomIn:
                targetFov = 80.0f;
                onTrigger = false;
                return;
            case ZoomType.DollyZoomOut:
                targetFov = 160.0f;
                onTrigger = true;
                return;
        }
    }

    void Update()
    {
        if (onTrigger)
        {
            zoomCamera(targetFov / zoomMultiplier);
        }
        else 
        {
            zoomCamera(targetFov);
        }
    }

    private void zoomCamera(float target)
    {
        float angle = Mathf.Abs((targetFov / zoomMultiplier) - targetFov);
        Camera.main.fieldOfView = Mathf.MoveTowards(Camera.main.fieldOfView, target, angle / zoomDuration * Time.deltaTime);
    }

}
