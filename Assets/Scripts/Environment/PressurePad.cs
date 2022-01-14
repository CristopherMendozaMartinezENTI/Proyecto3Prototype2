using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Connections { Door, Platform, ForceFild, DisableObject, Portal }

public class PressurePad : MonoBehaviour
{
    [Header("Settings")]
    [TagSelector] public string KeyTag = "";
    [SerializeField] private Connections ConnectedTo;
    [SerializeField] private GameObject connectedGameObject;
    private GameObject onTriggerVfx;

    private void Start()
    {
        onTriggerVfx = transform.GetChild(0).gameObject;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == KeyTag)
        {
            if(KeyTag == "KeyBlue")
                onTriggerVfx.GetComponent<ParticleSystem>().startColor = Color.cyan;

            if (KeyTag == "KeyRed")
                onTriggerVfx.GetComponent<ParticleSystem>().startColor = Color.red;
            
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
                case Connections.Portal:
                    connectedGameObject.GetComponent<BoxCollider>().enabled = true;
                    connectedGameObject.GetComponent<Portal>().GetRenderer().enabled = true;
                    connectedGameObject.GetComponent<Portal>().GetLinkedPortal().gameObject.GetComponent<BoxCollider>().enabled = true;
                    connectedGameObject.GetComponent<Portal>().GetLinkedPortal().GetRenderer().enabled = true;
                    return;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == KeyTag)
        {
            onTriggerVfx.GetComponent<ParticleSystem>().startColor = Color.white;
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
                case Connections.Portal:
                    connectedGameObject.GetComponent<BoxCollider>().enabled = false;
                    connectedGameObject.GetComponent<Portal>().GetRenderer().enabled = false;
                    connectedGameObject.GetComponent<Portal>().GetLinkedPortal().gameObject.GetComponent<BoxCollider>().enabled = false;
                    connectedGameObject.GetComponent<Portal>().GetLinkedPortal().GetRenderer().enabled = false;
                    return;
            }
        }
    }
}
