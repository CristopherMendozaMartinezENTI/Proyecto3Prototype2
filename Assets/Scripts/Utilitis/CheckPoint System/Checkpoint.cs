using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private CheckPointSystem cps;
    private GameObject player;

    private void Start()
    {
        cps = GameObject.FindGameObjectWithTag("CPS").GetComponent<CheckPointSystem>();
        player = GameObject.Find("Player");
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            cps.lastCheckPoint = this.transform.position;
            cps.lastRotation = player.transform.rotation;
        }
    }
}
