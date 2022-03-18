using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationPressurePad : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] public string KeyTag = "";
    [SerializeField] private GameObject connectedGameObject;
    [SerializeField] private Vector3 desiredRotation;
    [SerializeField] private bool rotationIsLocal;
    private Vector3 worldDesiredRotation;
    private Vector3 initialRotation;
    private GameObject onTriggerVfx;
    private bool returnToInitial;

    private void Start()
    {
        initialRotation = connectedGameObject.transform.eulerAngles;


        if (rotationIsLocal)
            worldDesiredRotation = desiredRotation + connectedGameObject.transform.localEulerAngles;
        else
            worldDesiredRotation = desiredRotation;

        worldDesiredRotation.x %= 360;
        worldDesiredRotation.y %= 360;
        worldDesiredRotation.z %= 360;

        onTriggerVfx = transform.GetChild(0).gameObject;
        returnToInitial = false;

        /*Debug.Log("EULER" + connectedGameObject.transform.eulerAngles);
        Debug.Log("LOCAL EULER" + connectedGameObject.transform.localEulerAngles);
        Debug.Log("ROTATION" + connectedGameObject.transform.rotation);
        Debug.Log("LOCAL ROTATION" + connectedGameObject.transform.localRotation);
        Debug.Log("DESIRED" + desiredRotation);*/
    }

    private void Update()
    {
        if (returnToInitial)
            if (connectedGameObject.transform.eulerAngles != initialRotation)
                connectedGameObject.transform.eulerAngles = Vector3.Lerp(connectedGameObject.transform.eulerAngles, initialRotation, Time.deltaTime * 5);
            else
                returnToInitial = false;        
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
                connectedGameObject.transform.eulerAngles = Vector3.Lerp(connectedGameObject.transform.eulerAngles, worldDesiredRotation, Time.deltaTime*5);

            returnToInitial = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == KeyTag)
        {
            onTriggerVfx.GetComponent<ParticleSystem>().startColor = Color.white;
            returnToInitial = true;
        }
    }
}
