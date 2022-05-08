using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformByInput : MonoBehaviour
{
    [Tooltip("If any of the Switches is active, rotation will trigger")]
    [SerializeField] private List<Switch> inputSwitches;
    [Tooltip("If any of the Pads is active, rotation will trigger")]
    [SerializeField] private List<PressurePad> inputPreassurePads;

    [Header("Object Rotation")]
    [Tooltip("If empty, will take 'this' as reference")]
    [SerializeField] private GameObject connectedGameObject;
    [Tooltip("Important to use world final rotation, not local. It is recomended have the object out of any parent")]
    [SerializeField] private Transform desiredTransform;
    /*[Tooltip("TRUE: the portal is in a parented space, and desired rotation is the one in that space\n" + 
        "FALSE: the portal is either in a parented space or not, and desired rotation is the world final rotation")]
    [SerializeField] private bool rotationIsLocal;*/

    //[Header("Parameters")]
    private Transform worldInitialTransform;
    private Transform worldDesiredTransform;


    private void Start()
    {
        if (connectedGameObject == null)
            connectedGameObject = gameObject;

        /*if (rotationIsLocal)
        {
            worldInitialRotation = connectedGameObject.transform.eulerAngles;
            //ConvertToStandardEuler(ref worldInitialRotation);
            worldDesiredRotation = desiredRotation + connectedGameObject.transform.localEulerAngles;
            //ConvertToStandardEuler(ref worldDesiredRotation);
            Debug.Log(worldDesiredRotation);
        }*/

        worldInitialTransform = connectedGameObject.transform;
        //ConvertToStandardEuler(ref worldInitialRotation);
        worldDesiredTransform = desiredTransform;
        //ConvertToStandardEuler(ref worldDesiredRotation);
    }

    private void Update()
    {
        //Check for active input, break immediately when true

        bool transform = false;

        foreach (Switch _switch in inputSwitches)
            if (!_switch.active) //la logica del switch va al reves ¯\_(ツ)_/¯
            {
        Debug.Log(_switch);
                transform = true;
                break;
            }

        if (!transform)
            foreach (PressurePad _pad in inputPreassurePads)
                if (_pad.active)
                {
                    transform = true;
                    break;
                }


        //Rotate game object accordingly

        /*if (transform)
            connectedGameObject.transform =
                LerpEulerAngles(connectedGameObject.transform, worldDesiredTransform, Time.deltaTime * 5);
        else
            connectedGameObject.transform.eulerAngles =
                LerpEulerAngles(connectedGameObject.transform.eulerAngles, worldInitialRotation, Time.deltaTime * 5);*/
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
    private Transform LerpEulerAngles(Transform tolerp, Transform goal, float dt)
    {
        Vector3 newPosition;
        newPosition.x = Mathf.Lerp(tolerp.transform.position.x, goal.transform.position.x, dt);
        newPosition.y = Mathf.Lerp(tolerp.transform.position.y, goal.transform.position.y, dt);
        newPosition.z = Mathf.Lerp(tolerp.transform.position.z, goal.transform.position.z, dt);

        Quaternion newRotation;
        newRotation.x = Mathf.LerpAngle(tolerp.eulerAngles.x, goal.eulerAngles.x, dt);
        newRotation.y = Mathf.LerpAngle(tolerp.eulerAngles.y, goal.eulerAngles.y, dt);
        newRotation.z = Mathf.LerpAngle(tolerp.eulerAngles.z, goal.eulerAngles.z, dt);
        
        //tolerp.transform.SetPositionAndRotation(newPosition, newRotation);
        return tolerp;
    }
}
