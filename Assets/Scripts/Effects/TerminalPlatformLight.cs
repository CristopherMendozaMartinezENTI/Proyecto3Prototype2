using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BOX_LIGHT { BOX, CENTER, FORWARD, RIGHT, LEFT, BACK };

public class TerminalPlatformLight : MonoBehaviour {

    private Platforms myPlatform;
    public Material mat;
    public Material transparent;

    private void Awake()
    {
        if (!GetComponent<Platforms>())
            DestroyImmediate(this);
        else myPlatform = GetComponent<Platforms>();
    }

    private void Update()
    {
        myPlatform.UpdateLightsGuides(GameObject.FindObjectOfType<CameraRigg>(),this);   
    }

    private void CheckStaticLight()
    {
            Material[] matArr = new Material[6];
            matArr = GetComponent<MeshRenderer>().materials;
            RaycastHit hit;

            for (int i = 2; i < 6; i++)
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
        Material[] matArr = new Material[6];
        matArr = GetComponent<MeshRenderer>().materials;

        if (enable)
            matArr[dir.GetHashCode()] = mat;
        else
            matArr[dir.GetHashCode()] = transparent;

        GetComponent<MeshRenderer>().materials = matArr;
    }

    public void ResetLight()
    {
        Material[] matArr = new Material[6];
        matArr = GetComponent<MeshRenderer>().materials;

        for(int i = 2; i < 6; i++)
        {
            matArr[i] = transparent;
        }
        GetComponent<MeshRenderer>().materials = matArr;

        CheckStaticLight();
    }
}
