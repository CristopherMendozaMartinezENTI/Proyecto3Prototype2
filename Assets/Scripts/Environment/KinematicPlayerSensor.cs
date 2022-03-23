using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicPlayerSensor : MonoBehaviour
{
    //This script is for chess pieces to not move when player is colliding

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }
    private void OnCollisionExit(Collision collision)
    {
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
    }
}
