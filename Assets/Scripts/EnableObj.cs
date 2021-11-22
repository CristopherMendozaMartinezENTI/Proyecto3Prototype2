using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableObj : MonoBehaviour
{
    [SerializeField] private GameObject obj;
    [SerializeField] private GameObject pressE;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            pressE.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                obj.SetActive(true);
                pressE.SetActive(false);
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
