using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationByInput : MonoBehaviour
{
    [Tooltip("If any of the Switches is active, rotation will trigger")]
    [SerializeField] private List<Switch> inputSwitches;
    [Tooltip("If any of the Pads is active, rotation will trigger")]
    [SerializeField] private List<PressurePad> inputPreassurePads;

    [Header("Object Rotation")]
    [Tooltip("If empty, will take 'this' as reference")]
    [SerializeField] private GameObject connectedGameObject;
    [Tooltip("Important to use world final rotation, not local. It is recomended have the object out of any parent")]
    [SerializeField] private Vector3 desiredRotation;
    /*[Tooltip("TRUE: the portal is in a parented space, and desired rotation is the one in that space\n" + 
        "FALSE: the portal is either in a parented space or not, and desired rotation is the world final rotation")]
    [SerializeField] private bool rotationIsLocal;*/

    //[Header("Parameters")]
    private Vector3 worldInitialRotation;
    private Vector3 worldDesiredRotation;


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

        worldInitialRotation = connectedGameObject.transform.eulerAngles;
        //ConvertToStandardEuler(ref worldInitialRotation);
        worldDesiredRotation = desiredRotation;
        //ConvertToStandardEuler(ref worldDesiredRotation);
    }

    private void Update()
    {
        //Check for active input, break immediately when true

        bool rotate = false;

        foreach (Switch _switch in inputSwitches)
            if (!_switch.active) //la logica del switch va al reves ¯\_(ツ)_/¯
            {
        //Debug.Log(_switch);
                rotate = true;
                break;
            }

        if (!rotate)
            foreach (PressurePad _pad in inputPreassurePads)
                if (_pad.active)
                {
                    rotate = true;
                    break;
                }


        //Rotate game object accordingly

        if (rotate)
            connectedGameObject.transform.eulerAngles =
                LerpEulerAngles(connectedGameObject.transform.eulerAngles, worldDesiredRotation, Time.deltaTime * 5);
        else
            connectedGameObject.transform.eulerAngles =
                LerpEulerAngles(connectedGameObject.transform.eulerAngles, worldInitialRotation, Time.deltaTime * 5);
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
