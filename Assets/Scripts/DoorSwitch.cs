using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSwitch : MonoBehaviour
{
    GameObject door;
    GameObject pressE;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            door.GetComponent<Animator>().Play("door_3_open");
            this.gameObject.GetComponent<BoxCollider>().enabled = false;
            pressE.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            pressE.SetActive(false);
        }
    }
}
