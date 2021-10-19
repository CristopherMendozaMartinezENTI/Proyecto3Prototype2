using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct FloatingPlatforms { public GameObject platformNeighbour; public int cameraView; }

public class Platforms : MonoBehaviour {

    public List<FloatingPlatforms> otherNeighbours = new List<FloatingPlatforms>();
    
    public List<GameObject> GetNeighbour()
    {
        List<GameObject> toReturn = new List<GameObject>();

        // Busquem i guardem totes les plataformes que estiguin enganxades
        float raydistance = 0.6f;
        RaycastHit hit;
        RaycastHit hit2;

        for (int i = 0; i < 4; i++)
        {
            Vector3 rayDirection = i == 0 ? Vector3.forward : i == 1 ? Vector3.right : i == 2 ? Vector3.back : Vector3.left;
            if (Physics.Raycast(transform.position, rayDirection, out hit, raydistance))
            {
                if (hit.collider.gameObject.GetComponent<Platforms>() && !hit.collider.gameObject.GetComponent<Platforms>().ArePawnOver())
                {
                    if (!Physics.Raycast(hit.collider.transform.position, Vector3.up, out hit2, Mathf.Infinity))
                    {
                        toReturn.Add(hit.collider.gameObject);
                    }
                }
            }
        }

        if (otherNeighbours.Count > 0)
        {
            for (int i = 0; i < otherNeighbours.Count; i++)
            {
                if (!Physics.Raycast(otherNeighbours[i].platformNeighbour.transform.position, Vector3.up, out hit, Mathf.Infinity))
                {

                    if (GameObject.FindObjectOfType<CameraRigg>().CurrentPos + 1 == otherNeighbours[i].cameraView)
                    {

                        if (GetComponent<DynamicPlatforms>() || otherNeighbours[i].platformNeighbour.GetComponent<DynamicPlatforms>())
                        {
                            if (GetComponent<DynamicPlatforms>() || otherNeighbours[i].platformNeighbour.GetComponent<DynamicPlatforms>())
                            {
                                GameObject camera = GameObject.FindObjectOfType<CameraRigg>().GetComponentInChildren<Camera>().gameObject;
                                Vector3 dir = camera.transform.TransformDirection(Vector3.forward);
                                Vector3 origin;

                                float inc = (otherNeighbours[i].platformNeighbour.transform.position.y - transform.position.y) / dir.y;

                                for (int j = 0; j < 4; j++)
                                {
                                    origin = transform.position + (j == 0 ? Vector3.forward : j == 1 ? Vector3.right : j == 2 ? Vector3.back : Vector3.left);
                                    Vector3 check = origin + (dir * inc);
                                    if (check == otherNeighbours[i].platformNeighbour.transform.position)
                                        toReturn.Add(otherNeighbours[i].platformNeighbour);
                                }

                            }

                            /*  GameObject cameraRigg = GameObject.FindObjectOfType<CameraRigg>().gameObject;
                              GameObject camera = cameraRigg.GetComponentInChildren<Camera>().gameObject;
                              GameObject near = Vector3.Distance(camera.transform.position, gameObject.transform.position) <
                                  Vector3.Distance(camera.transform.position, otherNeighbours[i].platformNeighbour.transform.position) ?
                                  gameObject : otherNeighbours[i].platformNeighbour;

                              Vector3 dir = camera.transform.TransformDirection(Vector3.forward);
                              Vector3 origin = near.transform.position + Vector3.up * 0.5f;

                              for (int j = 0; j < 4; j++)
                              {
                                  origin = near.transform.position + Vector3.up * 0.5f +
                                      (j == 0 ? Vector3.forward : j == 1 ? Vector3.right : j == 2 ? Vector3.back : Vector3.left);

                                  if (Physics.Raycast(origin, dir, out hit, Mathf.Infinity))
                                  {
                                      if (hit.collider.gameObject == (near == gameObject ? otherNeighbours[i].platformNeighbour : gameObject))
                                      {
                                          toReturn.Add(otherNeighbours[i].platformNeighbour);
                                      }
                                  }
                              }*/
                        }
                        else
                        {
                            toReturn.Add(otherNeighbours[i].platformNeighbour);
                        }
                    }
                }
            }
        }

        return toReturn;

    }

    public bool ArePawnOver()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.up, out hit, Mathf.Infinity))
        {
            return true;
        }
        else return false;
    }

    public bool IsNeighbour(GameObject obj)
    {
        bool toReturn = false;

        // Busquem i guardem totes les plataformes que estiguin enganxades
        float raydistance = 0.6f;
        RaycastHit hit;

        if (!Physics.Raycast(obj.transform.position,Vector3.up,out hit,Mathf.Infinity))
        {
            for (int i = 0; i < 4; i++)
            {
                Vector3 rayDirection = i == 0 ? Vector3.forward : i == 1 ? Vector3.right : i == 2 ? Vector3.back : Vector3.left;
                if (Physics.Raycast(transform.position, rayDirection, out hit, raydistance))
                {
                    if (hit.collider.gameObject == obj)
                        toReturn = true;
                }
            }

            if (otherNeighbours.Count > 0 && !toReturn)
            {

                for (int i = 0; i < otherNeighbours.Count; i++)
                {
                
                    if (GameObject.FindObjectOfType<CameraRigg>().CurrentPos + 1 == otherNeighbours[i].cameraView &&
                        otherNeighbours[i].platformNeighbour == obj)
                    {
                        
                        if (GetComponent<DynamicPlatforms>() || otherNeighbours[i].platformNeighbour.GetComponent<DynamicPlatforms>())
                        {
                            GameObject camera = GameObject.FindObjectOfType<CameraRigg>().GetComponentInChildren<Camera>().gameObject;
                            Vector3 dir = camera.transform.TransformDirection(Vector3.forward);
                            Vector3 origin;

                            float inc = (otherNeighbours[i].platformNeighbour.transform.position.y - transform.position.y) / dir.y;

                            for (int j = 0; j < 4; j++)
                            {
                                origin = transform.position + (j == 0 ? Vector3.forward : j == 1 ? Vector3.right : j == 2 ? Vector3.back : Vector3.left);
                                Vector3 check = origin + (dir * inc);
                                if (check == otherNeighbours[i].platformNeighbour.transform.position)
                                    toReturn = true;
                            }

                        }
                        /*GameObject cameraRigg = GameObject.FindObjectOfType<CameraRigg>().gameObject;
                             GameObject camera = cameraRigg.GetComponentInChildren<Camera>().gameObject;
                             GameObject near = Vector3.Distance(camera.transform.position, gameObject.transform.position) <
                                 Vector3.Distance(camera.transform.position, otherNeighbours[i].platformNeighbour.transform.position) ?
                                 gameObject : otherNeighbours[i].platformNeighbour;

                             Vector3 dir = camera.transform.TransformDirection(Vector3.forward);
                             Vector3 origin = near.transform.position + Vector3.up * 0.5f;

                             for (int j = 0; j < 4; j++)
                             {
                                 origin = near.transform.position + Vector3.up * 0.5f +
                                     (j == 0 ? Vector3.forward : j == 1 ? Vector3.right : j == 2 ? Vector3.back : Vector3.left);

                                 if (Physics.Raycast(origin, dir, out hit, Mathf.Infinity))
                                 {
                                     if (hit.collider.gameObject == (near == gameObject ? otherNeighbours[i].platformNeighbour : gameObject))
                                     {
                                         toReturn = true;
                                     }
                                 }
                             }*/
                        else
                        {
                            toReturn = true;
                        } 
                    }
                }
            }

        }
        return toReturn;

    }
        
    public void UpdateLightsGuides(CameraRigg cRigg, TerminalPlatformLight light)
    {
        light.ResetLight();

        if (otherNeighbours.Count > 0)
        {
            GameObject camera = cRigg.GetComponentInChildren<Camera>().gameObject;
            Vector3 dir = camera.transform.TransformDirection(Vector3.forward);
            Vector3 origin;

            for (int i = 0; i < otherNeighbours.Count; i++)
            {
                if (cRigg.CurrentPos + 1 == otherNeighbours[i].cameraView)
                {
                    float inc = (otherNeighbours[i].platformNeighbour.transform.position.y - transform.position.y) / dir.y;

                    for (int j = 0; j < 4; j++)
                    {
                        origin = transform.position + (j == 0 ? Vector3.forward : j == 1 ? Vector3.right : j == 2 ? Vector3.back : Vector3.left);
                        Vector3 check = origin + (dir * inc);
                        if (check == otherNeighbours[i].platformNeighbour.transform.position)
                            light.UpdateLight(true, (j == 0 ? BOX_LIGHT.FORWARD : j == 1 ? BOX_LIGHT.RIGHT : j == 2 ? BOX_LIGHT.BACK : BOX_LIGHT.LEFT));
                    }
                }
            }
        }
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (otherNeighbours.Count > 0)
        {
            Gizmos.color = Color.yellow;

            foreach (FloatingPlatforms link in otherNeighbours)
            {
                if (link.platformNeighbour)
                {
                    FloatingPlatforms check = new FloatingPlatforms();
                    check.platformNeighbour = gameObject;
                    check.cameraView = link.cameraView;

                    if (link.platformNeighbour.GetComponent<Platforms>().otherNeighbours.Contains(check))
                    {
                        Gizmos.DrawLine(transform.position, link.platformNeighbour.transform.position);
                        Vector3 cLine = (link.platformNeighbour.transform.position - transform.position) / 2;
                        UnityEditor.Handles.Label(transform.position + cLine, link.cameraView.ToString());
                    }
                    else
                    {
                        Vector3 cLine = (link.platformNeighbour.transform.position - transform.position) / 2;
                        Gizmos.DrawLine(transform.position, transform.position + cLine);
                        Gizmos.color = new Color(1, 0, 0, 0.5f);
                        Gizmos.DrawCube(transform.position + cLine, Vector3.one * 0.3f);
                    }
                }
            }
        }
    }
#endif
}



