using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayUiElement : MonoBehaviour
{
    [SerializeField] List<GameObject> uiElements;
    [SerializeField] bool isTimed;
    [SerializeField] float activeSeconds = 0;

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            foreach(GameObject uiElement in uiElements)
            {
                uiElement.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            foreach (GameObject uiElement in uiElements)
            {
                uiElement.SetActive(false);
            }
        }
    }
}
