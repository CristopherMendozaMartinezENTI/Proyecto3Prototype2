using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticDoor : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            this.gameObject.GetComponent<Animator>().Play("Door_open");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            this.gameObject.GetComponent<Animator>().Play("Door_opened");
        }
    }
}
