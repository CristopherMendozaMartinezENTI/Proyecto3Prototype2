using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject pressECanvas;
    [SerializeField] private Connections ConnectedTo;
    [SerializeField] private GameObject connectedGameObject;
    public bool active { get; private set; } = true; // on / off
    private bool triggerStay = false;
    private bool keyPressed = false;
    private Material[] matArray;
    private Material screenOn;
    private Material screenOff;
    private MeshRenderer mesh;

    private void Start()
    {
        mesh = this.gameObject.GetComponent<MeshRenderer>();
        matArray = mesh.materials;
        screenOn = Resources.Load<Material>(@"Materials/" + "OnMat");
        screenOff = Resources.Load<Material>(@"Materials/" + "OffMat");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && triggerStay)
        {
            keyPressed = true;
            active = !active;
            gameObject.GetComponent<AudioSource>().Play();
        }

        if (Input.GetKeyUp(KeyCode.E) && triggerStay)
        {
            keyPressed = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            triggerStay = true;
            pressECanvas.SetActive(true);
            if (keyPressed)
            {
                if (!active)
                {
                    matArray[1] = screenOn;
                    mesh.materials = matArray;
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
                else
                {
                    matArray[1] = screenOff;
                    mesh.materials = matArray;
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
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            pressECanvas.SetActive(false);
            triggerStay = false;
        }
    }
}
