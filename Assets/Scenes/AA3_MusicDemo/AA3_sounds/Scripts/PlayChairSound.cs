using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayChairSound : MonoBehaviour
{
    private AudioSource audioComponent;
    public AudioClip[] chairRocking;
    public float fvolMin = 0.03f, fvolMax = 0.15f;
    public float fPitchMin = 0.8f, fPitchMax = 1.2f;
    // Start is called before the first frame update
    void Start()
    {
        audioComponent = GetComponent<AudioSource>();

    }

    public void PlayChairClip1()
    {
        audioComponent.volume = Random.Range(fvolMin, fvolMax);
        audioComponent.pitch = Random.Range(fPitchMin, fPitchMax);
        audioComponent.PlayOneShot(chairRocking[Random.Range(0, chairRocking.Length)]);
    }
}
