using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Connections { Lvl1Door, Platform, ForceFild }

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
                case Connections.Lvl1Door:
                    connectedGameObject.GetComponent<Animator>().Play("door_3_open");
                    return;
                case Connections.Platform:
                    connectedGameObject.GetComponent<MovingPlatform>().movementEnabled = true;
                    return;
                case Connections.ForceFild:
                    //connectedGameObject.GetComponent<BoxCollider>().enabled = false;
                    connectedGameObject.SetActive(false);
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
                case Connections.Lvl1Door:
                    connectedGameObject.GetComponent<Animator>().Play("door_3_opened");
                    return;
                case Connections.Platform:
                    connectedGameObject.GetComponent<MovingPlatform>().movementEnabled = false;
                    return;
                case Connections.ForceFild:
                    //connectedGameObject.GetComponent<BoxCollider>().enabled = true;
                    connectedGameObject.SetActive(true);
                    return;
            }
        }
    }
}
