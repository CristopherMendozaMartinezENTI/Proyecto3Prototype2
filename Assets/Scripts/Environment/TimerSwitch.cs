using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerSwitch : MonoBehaviour
{
   [Header("Settings")]
    [SerializeField] private GameObject pressECanvas;
    [SerializeField] private Connections ConnectedTo;
    [SerializeField] private GameObject connectedGameObject;
    [SerializeField] private float activeTime = 5.0f;
    private bool triggerStay = false;
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
            gameObject.GetComponent<AudioSource>().Play();
            StartCoroutine(WaitForDesactivate());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            triggerStay = true;
            pressECanvas.SetActive(true);
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

    IEnumerator WaitForDesactivate()
    {
        pressECanvas.SetActive(false);
        triggerStay = false;
        gameObject.GetComponent<BoxCollider>().enabled = false;
        Active();
        yield return new WaitForSeconds(activeTime);
        Desactive();
        gameObject.GetComponent<BoxCollider>().enabled = true;
    }

    private void Active()
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

    private void Desactive()
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
