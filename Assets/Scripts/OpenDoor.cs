using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            this.gameObject.GetComponent<Animator>().Play("door_3_open");
            this.gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }
}
