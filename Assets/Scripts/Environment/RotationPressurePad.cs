using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationPressurePad : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] public string KeyTag = "";
    [SerializeField] private GameObject connectedGameObject;
    [SerializeField] private Vector3 desiredRotation;
    private Vector3 initialRotation;
    private GameObject onTriggerVfx;

    private void Start()
    {
        initialRotation = connectedGameObject.transform.eulerAngles;
        onTriggerVfx = transform.GetChild(0).gameObject;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == KeyTag)
        {
            if (KeyTag == "KeyBlue")
                onTriggerVfx.GetComponent<ParticleSystem>().startColor = Color.cyan;

            if (KeyTag == "KeyRed")
                onTriggerVfx.GetComponent<ParticleSystem>().startColor = Color.red;

            if (KeyTag == "KeyGreen")
                onTriggerVfx.GetComponent<ParticleSystem>().startColor = Color.green;

            if (KeyTag == "KeyPurple")
                onTriggerVfx.GetComponent<ParticleSystem>().startColor = new Color(148, 0, 211);

            if (connectedGameObject.transform.eulerAngles != desiredRotation)
                connectedGameObject.transform.eulerAngles = Vector3.Lerp(connectedGameObject.transform.eulerAngles, desiredRotation, Time.deltaTime*5);
           
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == KeyTag)
        {
            onTriggerVfx.GetComponent<ParticleSystem>().startColor = Color.white;
            connectedGameObject.transform.eulerAngles = initialRotation;
        }
    }
}
