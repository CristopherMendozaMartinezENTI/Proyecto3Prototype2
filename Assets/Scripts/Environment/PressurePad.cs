using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Connections { Door, Platform, ForceFild, DisableObject }

public class PressurePad : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Connections ConnectedTo;
    [SerializeField] private GameObject connectedGameObject;
    private GameObject onTriggerVfx;

    private void Start()
    {
        onTriggerVfx = transform.GetChild(0).gameObject;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Key")
        {
            onTriggerVfx.GetComponent<ParticleSystem>().startColor = Color.cyan;
            switch (ConnectedTo)
            {
                case Connections.Door:
                    connectedGameObject.GetComponent<Animator>().Play("Door_open");
                    return;
                case Connections.Platform:
                    connectedGameObject.GetComponent<MovingPlatform>().movementEnabled = true;
                    return;
                case Connections.ForceFild:
                    connectedGameObject.SetActive(false);
                    return;
                case Connections.DisableObject:
                    connectedGameObject.SetActive(true);
                    return;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Key")
        {
            onTriggerVfx.GetComponent<ParticleSystem>().startColor = Color.red;
            switch (ConnectedTo)
            {
                case Connections.Door:
                    connectedGameObject.GetComponent<Animator>().Play("Door_opened");
                    return;
                case Connections.Platform:
                    connectedGameObject.GetComponent<MovingPlatform>().movementEnabled = false;
                    return;
                case Connections.ForceFild:
                    connectedGameObject.SetActive(true);
                    return;
                case Connections.DisableObject:
                    connectedGameObject.SetActive(false);
                    return;
            }
        }
    }
}
