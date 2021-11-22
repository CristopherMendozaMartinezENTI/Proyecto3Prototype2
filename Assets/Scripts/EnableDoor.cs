using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableDoor : MonoBehaviour
{
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject pressE;
    [SerializeField] private GameObject uiIcon;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            pressE.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                pressE.SetActive(false);
                this.gameObject.GetComponent<AudioSource>().Play();
                door.GetComponent<BoxCollider>().enabled = true;
                this.gameObject.SetActive(false);
                uiIcon.SetActive(true);
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
