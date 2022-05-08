using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindWall : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            if (other.gameObject.GetComponent<AutomaticRewind>().enabled)
            {
                other.gameObject.GetComponent<AutomaticRewind>().enabled = false;
            }
            else
            {
                other.gameObject.GetComponent<AutomaticRewind>().enabled = true;
            }
        }
    }
}
