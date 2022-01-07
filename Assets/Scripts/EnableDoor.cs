using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableDoor : MonoBehaviour
{
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject pressE;
    private bool triggerStay = false;
    private bool keyPressed = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && triggerStay)
        {
            keyPressed = true;
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
            pressE.SetActive(true);
            triggerStay = true;
            if (keyPressed)
            {
                pressE.SetActive(false);
                door.GetComponent<BoxCollider>().enabled = true;
                gameObject.GetComponent<MeshRenderer>().enabled = false;
                gameObject.GetComponent<BoxCollider>().enabled = false;
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
