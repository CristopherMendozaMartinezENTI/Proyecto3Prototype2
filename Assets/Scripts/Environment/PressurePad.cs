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
    private ParticleSystem ps;
    private ParticleSystem.MainModule main;

    private void Start()
    {
        ps = transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
        main = ps.main;
    }

    private void Update()
    {
        if(active)
        {
            if (KeyTag == "KeyBlue")
                main.startColor = Color.cyan;

            if (KeyTag == "KeyGreen")
                main.startColor = Color.green;

            if (KeyTag == "KeyPurple")
                main.startColor = new Color(0.5f, 0, 1);

            if (KeyTag == "KeyOrange")
                main.startColor = new Color(1, 0.3f, 0);

            if (KeyTag == "KeyYelow")
                main.startColor = new Color(1, 1, 0);

            if (KeyTag == "KeyPink")
                main.startColor = new Color(1, 0.3f, 0.8f);
        }

       else
       {
            main.startColor = Color.white;
       }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == KeyTag)
        {
            active = true;

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
