using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalScaff : MonoBehaviour
{
    void Update()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = GetComponent<ActiveStateManager>().active;
    }
}
