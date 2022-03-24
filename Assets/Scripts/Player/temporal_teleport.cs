using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class temporal_teleport : MonoBehaviour
{
    [SerializeField] Vector3 teleportPosition;

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.T))
            gameObject.transform.position = teleportPosition;
    }

}
