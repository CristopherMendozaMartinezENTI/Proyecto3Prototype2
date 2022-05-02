using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AutomaticDoor_mod : MonoBehaviour
{
    [SerializeField] private AudioClip openDoorClip;
    private AudioSource audioComponent;
    public float fvolMin = 0.15f, fvolMax = 0.25f;
    public float fPitchMin = 0.8f, fPitchMax = 1.2f;
    private void Start()
    {
        audioComponent = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            audioComponent.volume = Random.Range(fvolMin, fvolMax);
            audioComponent.pitch = Random.Range(fPitchMin, fPitchMax);
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
