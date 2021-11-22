using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenGlassDoor : MonoBehaviour
{
    GameObject pressE;
    // Start is called before the first frame update
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            this.gameObject.GetComponent<Animator>().Play("glass_door_open");
            this.gameObject.GetComponent<BoxCollider>().enabled = false;
            pressE.SetActive(true);
        }
    }
}
