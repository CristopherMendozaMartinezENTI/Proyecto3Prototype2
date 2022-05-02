using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAudio : MonoBehaviour
{
    public List<AudioClip> audioclips;
    private AudioSource audioComponent;

    public float fRandomMin = 2.0f, fRandomMax = 8.0f, fRandomTime = 0.0f;
    public float fvolMin = 0.03f, fvolMax = 0.15f;
    public float fPitchMin = 0.8f, fPitchMax = 1.2f;

    // Start is called before the first frame update
    void Start()
    {
        audioComponent = GetComponent<AudioSource>();
        StartCoroutine(RandomPlay(fRandomMin, fRandomMax, fRandomTime, fvolMin, fvolMax, fPitchMin, fPitchMax, audioComponent, audioclips));
    }

    static IEnumerator RandomPlay(float fRandomMin, float fRandomMax, float fRandomTime, float fvolMin, float fvolMax, float fPitchMin, float fPitchMax, AudioSource audioComponent, List<AudioClip> audioclips) 
    {
        while (true) 
        {
            fRandomTime = Random.Range(fRandomMin, fRandomMax);
            audioComponent.clip = audioclips[Random.Range(0, audioclips.Count)];
            audioComponent.volume = Random.Range(fvolMin, fvolMax);
            audioComponent.pitch = Random.Range(fPitchMin, fPitchMax);
            audioComponent.Play();

            yield return new WaitForSeconds(fRandomTime);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
