using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostRing : MonoBehaviour
{
    [SerializeField] private float boostForce;
    [SerializeField] private Transform orientation;
    private Collider objectCollider;

    public bool delayedForward;
    public bool forward = true;
    private void OnTriggerEnter(Collider _collider)
    {
        if (_collider.CompareTag("Player") && forward)
            _collider.GetComponent<Rigidbody>().AddForce(orientation.forward * boostForce);
        if (_collider.CompareTag("Player") && delayedForward){
            _collider.GetComponent<Rigidbody>().AddForce(orientation.up * boostForce / 3);
            objectCollider = _collider;
            Invoke("Delay", .1f);
        }
        if (_collider.CompareTag("Player") && !forward)
            _collider.GetComponent<Rigidbody>().AddForce(orientation.up * boostForce);
    }

    private void Delay()
    {
        objectCollider.GetComponent<Rigidbody>().AddForce(orientation.forward * boostForce);
    }
}
