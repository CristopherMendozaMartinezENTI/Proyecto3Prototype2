using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionPlay : MonoBehaviour
{
    private AudioSource mySource;

    public float fvolMin = 0.7f, fvolMax = 1.0f;
    public float fPitchMin = 0.9f, fPitchMax = 1.2f;

    // Start is called before the first frame update
    void Start()
    {
        mySource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player") 
        {
            mySource.volume = Random.Range(fvolMin, fvolMax);
            mySource.pitch = Random.Range(fPitchMin, fPitchMax);
            mySource.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
