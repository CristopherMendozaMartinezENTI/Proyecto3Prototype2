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
    public Vector3 worldDesiredRotation;
    public Vector3 worldInitialRotation;
    private bool returnToInitial;

    private GameObject onTriggerVfx;

    private void Start()
    {
        onTriggerVfx = transform.GetChild(0).gameObject;

        if (rotationIsLocal)
        {
            worldInitialRotation = connectedGameObject.transform.localEulerAngles;
            ConvertToEuler(ref worldInitialRotation);
            worldDesiredRotation = desiredRotation + worldInitialRotation;
            ConvertToEuler(ref worldDesiredRotation);
        }
        else
        {
            worldInitialRotation = connectedGameObject.transform.eulerAngles;
            ConvertToEuler(ref worldInitialRotation);
            worldDesiredRotation = desiredRotation;
            ConvertToEuler(ref worldDesiredRotation);
        }

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
            if (connectedGameObject.transform.eulerAngles != worldInitialRotation)
                connectedGameObject.transform.eulerAngles = Vector3.Lerp(connectedGameObject.transform.eulerAngles, worldInitialRotation, Time.deltaTime * 5);
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

            if (connectedGameObject.transform.eulerAngles != worldDesiredRotation)
                connectedGameObject.transform.eulerAngles = Vector3.Lerp(connectedGameObject.transform.eulerAngles, worldDesiredRotation, Time.deltaTime * 5);

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

    private void ConvertToEuler(ref Vector3 vector3)
    {
        //-359 to 359
        vector3.x %= 360;
        vector3.y %= 360;
        vector3.z %= 360;

        //0 to 359x2
        vector3.x += 360;
        vector3.y += 360;
        vector3.z += 360;

        //0 to 359
        vector3.x %= 360;
        vector3.y %= 360;
        vector3.z %= 360;
    }
}
