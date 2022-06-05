using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformByInput : MonoBehaviour
{
    [SerializeField] private int activeLevel;
    [SerializeField] private bool usingLocalCoords;
    [Header("Desired Transform")]
    [Tooltip("World Position")]
    [SerializeField] private Vector3 position;
    [SerializeField] private bool IgnorePosition;
    [SerializeField] private Vector3 rotation;
    [SerializeField] private bool IgnoreRotation;
    [SerializeField] private Vector3 scale;
    [SerializeField] private bool IgnoreScale;

    //
    private GameObject connectedGameObject;

    private Vector3 worldInitialPosition;
    private Vector3 worldInitialRotation;
    private Vector3 worldInitialScale;

    private Vector3 worldDesiredPosition;
    private Vector3 worldDesiredRotation;
    private Vector3 worldDesiredScale;


    private void Start()
    {
        if (connectedGameObject == null)
            connectedGameObject = gameObject;

        worldInitialPosition = connectedGameObject.transform.position;
        worldInitialRotation = connectedGameObject.transform.eulerAngles;
        worldInitialScale = connectedGameObject.transform.localScale;


        worldDesiredPosition = position;
        if (usingLocalCoords && transform.parent)
            worldDesiredPosition += transform.parent.position;
        worldDesiredRotation = rotation;
        worldDesiredScale = scale;
    }

    private void Update()
    {
        bool transform = false;

        if (GetComponent<ActiveStateManager>())
            transform = GetComponent<ActiveStateManager>().active;
        if (GetComponent<ActiveHirearchyManager>())
            if (GetComponent<ActiveHirearchyManager>().currentLevel >= activeLevel)
            transform = true;


        //Transform game object accordingly

        if (!IgnorePosition)
            if (transform)
                connectedGameObject.transform.position =
                    Vector3.Slerp(connectedGameObject.transform.position, worldDesiredPosition, Time.deltaTime * 5);
            else
                connectedGameObject.transform.position =
                    Vector3.Slerp(connectedGameObject.transform.position, worldInitialPosition, Time.deltaTime * 5);

        if (!IgnoreRotation)
            if (transform)
                connectedGameObject.transform.eulerAngles =
                    LerpEulerAngles(connectedGameObject.transform.eulerAngles, worldDesiredRotation, Time.deltaTime * 5);
            else
                connectedGameObject.transform.eulerAngles =
                    LerpEulerAngles(connectedGameObject.transform.eulerAngles, worldInitialRotation, Time.deltaTime * 5);

        if (!IgnoreScale)
            if (transform)
                connectedGameObject.transform.localScale =
                    Vector3.Slerp(connectedGameObject.transform.localScale, worldDesiredScale, Time.deltaTime * 5);
            else
                connectedGameObject.transform.localScale =
                    Vector3.Slerp(connectedGameObject.transform.localScale, worldInitialScale, Time.deltaTime * 5);

    }

    //Converts any angle to its equivalent between 0 and 359
    private void ConvertToStandardEuler(ref Vector3 vector3)
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

    //Esto soluciona que flipas los problemas de rotacion ( this makes ConvertToStandardEuler useless )
    private Vector3 LerpEulerAngles(Vector3 tolerp, Vector3 goal, float dt)
    {
        tolerp.x = Mathf.LerpAngle(tolerp.x, goal.x, dt);
        tolerp.y = Mathf.LerpAngle(tolerp.y, goal.y, dt);
        tolerp.z = Mathf.LerpAngle(tolerp.z, goal.z, dt);

        return tolerp;
    }
}
