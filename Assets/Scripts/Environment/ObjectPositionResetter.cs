using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPositionResetter : MonoBehaviour
{
    [SerializeField] private GameObject playerTelekinesis;

    public void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PropPhysicsController>() != null)
        {
            other.GetComponent<PropPhysicsController>().ResetPos();
            if (playerTelekinesis.GetComponent<TelekinesisController>().IsObjectGrabbed())
                playerTelekinesis.GetComponent<TelekinesisController>().ReleaseObject();
        }
    }
}
