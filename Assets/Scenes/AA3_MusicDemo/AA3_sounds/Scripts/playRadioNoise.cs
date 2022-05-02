using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playRadioNoise : MonoBehaviour
{
    private AudioSource audioComponent;
    // Start is called before the first frame update
    private void Start()
    {
        audioComponent = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            audioComponent.loop = true;
            audioComponent.Play();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            audioComponent.loop = false;
        }
    }
}

