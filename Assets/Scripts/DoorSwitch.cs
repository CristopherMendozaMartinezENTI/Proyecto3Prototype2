using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSwitch : MonoBehaviour
{
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject pressE;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            pressE.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                door.GetComponent<Animator>().Play("door_3_open");
                pressE.SetActive(false);
                this.gameObject.GetComponent<AudioSource>().Play();
                this.gameObject.GetComponent<BoxCollider>().enabled = false;
            }
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
