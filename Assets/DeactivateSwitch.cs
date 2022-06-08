using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateSwitch : MonoBehaviour
{
    void Update()
    {
        if (GetComponent<Switch>().active)
            GetComponent<BoxCollider>().enabled = false;
    }
}
