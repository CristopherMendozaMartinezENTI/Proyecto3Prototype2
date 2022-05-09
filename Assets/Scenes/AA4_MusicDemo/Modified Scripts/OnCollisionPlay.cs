using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionPlay : MonoBehaviour
{
    private void Start()
    {
        
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.tag == "Player") 
            AkSoundEngine.PostEvent("Play_Land", gameObject);
    }
    private void Update()
    {
        
    }
}
