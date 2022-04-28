using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Oclusion : MonoBehaviour
{
    Transform playerPosition;
    public LayerMask occlusionLayer = 1;

    public AudioMixerSnapshot occluded;
    public AudioMixerSnapshot nonOccluded;

    // Start is called before the first frame update
    void Awake()
    {
        playerPosition = GameObject.FindObjectOfType<AudioListener>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Physics.Linecast(transform.position, playerPosition.position, out hit, occlusionLayer);

        if (hit.collider.tag == "Player")
        {
            nonOccluded.TransitionTo(.7f);
        }
        else 
        {
            occluded.TransitionTo(.7f);
        }

    }
}
