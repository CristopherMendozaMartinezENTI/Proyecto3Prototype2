using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntimatterForceField : MonoBehaviour
{
    [SerializeField] private GameObject collisionTrailsPrefab;
    private GameObject playerTelekinesis;
    private GameObject _collisionTrailsTmp;

    public void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PropPhysicsController>() != null)
        {
            _collisionTrailsTmp = Instantiate(collisionTrailsPrefab, other.transform.position, other.transform.rotation);
            other.GetComponent<PropPhysicsController>().ResetPos();
            if (playerTelekinesis.GetComponent<TelekinesisController>().IsObjectGrabbed())
                playerTelekinesis.GetComponent<TelekinesisController>().ReleaseObject();
        }
    }

    private void Start()
    {
        playerTelekinesis = GameObject.Find("TelekinesisGaunlet");
    }

    private void Update()
    {
        Destroy(_collisionTrailsTmp, 5.0f);
    }
}
