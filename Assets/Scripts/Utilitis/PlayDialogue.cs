using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayDialogue : MonoBehaviour
{
    [SerializeField] private List<AudioSource> sources;
    private bool isTriggered;

    void Update()
    {
        if(isTriggered)
        {
            isTriggered = false;
            gameObject.GetComponent<BoxCollider>().enabled = false;
            StartCoroutine(PlayClips());
        }    
    }

    IEnumerator PlayClips()
    {
        foreach (AudioSource source in sources)
        {
            source.Play();
           
            while (source.isPlaying)
            {
                yield return null;
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            isTriggered = true;
        }
    }
}
