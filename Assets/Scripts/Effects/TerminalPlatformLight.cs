using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BOX_LIGHT { BOX, TOP , CENTER , FORWARD, RIGHT, LEFT, BACK, MAX};

public class TerminalPlatformLight : MonoBehaviour {
    
    public Material mat;
    public Material transparent;
    
    private void Start()
    {
        if (!GetComponent<Platforms>())
            DestroyImmediate(this);
        GetComponent<Platforms>().UpdateLightsGuides();
    }
    private void CheckStaticLight()
    {
            Material[] matArr = GetComponent<MeshRenderer>().materials;
            RaycastHit hit;

            for (int i = BOX_LIGHT.FORWARD.GetHashCode(); i < BOX_LIGHT.MAX.GetHashCode(); i++)
            {
                Vector3 rayDirection = (i == BOX_LIGHT.FORWARD.GetHashCode() ? Vector3.forward :
                    i == BOX_LIGHT.RIGHT.GetHashCode() ? Vector3.right :
                    i == BOX_LIGHT.BACK.GetHashCode() ? Vector3.back :
                    Vector3.left);

                if (Physics.Raycast(transform.position, rayDirection, out hit, 0.505f))
                {
                    if (hit.collider.tag == "Platform")
                    {
                        matArr[i] = mat;
                    }
                }
            }
            GetComponent<MeshRenderer>().materials = matArr;
    }

    public void UpdateLight(bool enable, BOX_LIGHT dir)
    {
        Material[] matArr = GetComponent<MeshRenderer>().materials;

        if (enable)
            matArr[dir.GetHashCode()] = mat;
        else
            matArr[dir.GetHashCode()] = transparent;

        GetComponent<MeshRenderer>().materials = matArr;
    }

    public void ResetLight()
    {
        Material[] matArr = GetComponent<MeshRenderer>().materials;
        
        for (int i = BOX_LIGHT.FORWARD.GetHashCode(); i < BOX_LIGHT.MAX.GetHashCode(); i++)
        {
            matArr[i] = transparent;
        }
        GetComponent<MeshRenderer>().materials = matArr;
      
        CheckStaticLight();
    }
}
