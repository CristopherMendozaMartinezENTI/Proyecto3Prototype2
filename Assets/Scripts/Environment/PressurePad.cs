using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Connections { Door, Platform, ForceFild, DisableObject, Portal, None }

public class PressurePad : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] public string KeyTag = "";
    [SerializeField] private Connections ConnectedTo;
    [SerializeField] private GameObject connectedGameObject;
    [SerializeField] private bool invertedAMFF;
    public bool active { get; private set; } = false;
    private GameObject onTriggerVfx;

    private void Start()
    {
        onTriggerVfx = transform.GetChild(0).gameObject;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == KeyTag)
        {
            active = true;

            if (KeyTag == "KeyBlue")
                onTriggerVfx.GetComponent<ParticleSystem>().startColor = Color.cyan;

            if (KeyTag == "KeyRed")
                onTriggerVfx.GetComponent<ParticleSystem>().startColor = new Color(255, 69, 0);

            if (KeyTag == "KeyGreen")
                onTriggerVfx.GetComponent<ParticleSystem>().startColor = Color.green;

            if (KeyTag == "KeyPurple")
                onTriggerVfx.GetComponent<ParticleSystem>().startColor =  new Color(148, 0, 211);

            switch (ConnectedTo)
            {
                case Connections.Door:
                    connectedGameObject.GetComponent<Animator>().Play("Door_open");
                    return;
                case Connections.Platform:
                    connectedGameObject.GetComponent<MovingPlatform>().movementEnabled = true;
                    return;
                case Connections.ForceFild:
                    connectedGameObject.SetActive(invertedAMFF);
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
                case Connections.None:
                    return;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == KeyTag)
        {
            active = false;

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
                    connectedGameObject.SetActive(!invertedAMFF);
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
                case Connections.None:
                    return;
            }
        }
    }
}
