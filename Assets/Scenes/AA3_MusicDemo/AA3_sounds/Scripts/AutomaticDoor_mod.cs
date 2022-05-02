using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AutomaticDoor_mod : MonoBehaviour
{
    [SerializeField] private AudioClip openDoorClip;
    private AudioSource audioComponent;
    private void Start()
    {
        audioComponent = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            audioComponent.PlayOneShot(openDoorClip);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
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
