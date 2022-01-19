using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseDoor : MonoBehaviour
{
    [SerializeField] private GameObject door;
    [SerializeField] private PressurePad _pressurePad;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            door.GetComponent<Animator>().Play("Door_opened");
            _pressurePad.enabled = false;
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
           //
        }
    }
}
