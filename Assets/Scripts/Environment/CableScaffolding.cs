using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CableScaffolding : MonoBehaviour
{
    private Material[] matArray;
    private Material screenOn;
    private Material screenOff;



    void Start()
    {

        screenOn = Resources.Load<Material>(@"Materials/" + "OnMat");
        screenOff = Resources.Load<Material>(@"Materials/" + "OffMat");
    }

    void Update()
    {

        //matArray[1]= GetComponent<LineRenderer>().materials;
        GetComponent<MeshRenderer>().materials = matArray;
    }
}
