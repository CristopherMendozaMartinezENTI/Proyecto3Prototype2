using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Pawns : MonoBehaviour {

    List<GameObject> path = new List<GameObject>();
    private Vector3 newPos= Vector3.one * 10000;

    private TerminalGameManager myGm;
    public TerminalGameManager PawmGm
    {
        set { myGm = value; }
    }

    public bool IsMoving()
    {
        if (path.Count != 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Update()
    {
        if (path.Count > 0)
        {
            if (newPos == Vector3.one * 10000)
            {
                
                if (MainPlatform().GetComponent<Platforms>().IsNeighbour(path[0],true))
                {
                    if (path[0].transform.position.y - (transform.position.y - 1.0f) > 0.1f)
                    {
                        Vector3 camAngle = myGm.MainCam.transform.TransformDirection(Vector3.back);
                        float dif = ((path[0].transform.position.y + 1.0f) - transform.position.y) / camAngle.y;
                        transform.position = new Vector3(transform.position.x + (camAngle.x * dif),
                            path[0].transform.position.y + 1.0f,
                            transform.position.z + (camAngle.z * dif));

                        newPos = path[0].transform.position;
                    }
                    else if (path[0].transform.position.y - MainPlatform().transform.position.y < -0.1f)
                    {
                        Vector3 camAngle = myGm.MainCam.transform.TransformDirection(Vector3.back);
                        float dif = (transform.position.y - (path[0].transform.position.y)) / camAngle.y;
                        newPos = new Vector3(path[0].transform.position.x + (camAngle.x * dif),
                             transform.position.y,
                             path[0].transform.position.z + (camAngle.z * dif)) - camAngle;
                    }
                    else
                    {
                        newPos = path[0].transform.position;
                       
                    }
                }
                else
                    path.Clear();
            }
            
            Vector3 dir = (newPos + Vector3.up) - transform.position;
            float dist = dir.magnitude;
            dir = dir.normalized;

            if (dist > 0.05f)
            {

                transform.position += dir * 2.0f * Time.deltaTime;
            }
            else
            {
                if (newPos != path[0].transform.position + Vector3.up)
                    transform.position = path[0].transform.position + Vector3.up;

                //CheckRenderQueue(myGm.MainCam);
                path.RemoveAt(0);
                newPos = Vector3.one * 10000;
                
            }


        }
    }

    public void RepositionCharacter(GameObject destiny)
    {
        if (path.Count == 0 && destiny != MainPlatform())
            path = SearchPath(MainPlatform(), destiny);
    }

    private GameObject MainPlatform()
    {
        GameObject platform = null;
        RaycastHit hit;
        if (Physics.Raycast(transform.position,Vector3.down,out hit, Mathf.Infinity))
        {
            platform = hit.collider.gameObject;
        }
        return platform;
    }

    class PathNode
    {
        public PathNode(GameObject _my, PathNode _from) { my = _my; from = _from; }
        public GameObject my;
        public PathNode from;
    }

    private List<GameObject> SearchPath(GameObject mainPlatform, GameObject destiny)
    { 
        List<PathNode> path = new List<PathNode>();
        List<PathNode> checkList = new List<PathNode>();
        List<GameObject> included = new List<GameObject>();
        PathNode correctLast = new PathNode(mainPlatform,null);
        included.Add(mainPlatform);checkList.Add(new PathNode(mainPlatform, null));
        path.Add(new PathNode(mainPlatform, null));
        do
        {
            if (checkList[0].my.GetComponent<Platforms>().GetNeighbour().Count > 0)
            {
                foreach (GameObject node in checkList[0].my.GetComponent<Platforms>().GetNeighbour())
                {
                    if (!included.Contains(node))
                    {
                        included.Add(node);
                        PathNode newNode = new PathNode(node, checkList[0]);
                        path.Add(newNode);
                        checkList.Add(newNode);
                        if (node == destiny)
                            correctLast = newNode;
                    }
                }
            }

            checkList.RemoveAt(0);
        } while (checkList.Count > 0 && correctLast.my != destiny);

        List<GameObject> shortPath = new List<GameObject>();
        if (correctLast.my.name == destiny.name)
        {
            PathNode nextNode = correctLast;
            do
            {
                shortPath.Add(nextNode.my);
                nextNode = nextNode.from;
            } while (nextNode.from != null);

            shortPath.Reverse();
            return shortPath;
        }
        else
        {
            return new List<GameObject>();
        }
    }
    
    public void CheckRenderQueue(GameObject camera)
    {
        GameObject mainplat = MainPlatform();

        bool ctrl = false;

        foreach (FloatingPlatforms neightbour in mainplat.GetComponent<Platforms>().otherNeighbours)
        {
            if (mainplat.GetComponent<Platforms>().IsNeighbour(neightbour.platformNeighbour, false))
            {
                ctrl = true;
                Vector2 nPos = camera.GetComponent<Camera>().WorldToScreenPoint(neightbour.platformNeighbour.transform.position + Vector3.up * .5f);
                Vector2 pPos = camera.GetComponent<Camera>().WorldToScreenPoint(transform.position);

                if (nPos.y > pPos.y)
                {
                    GetComponent<MeshRenderer>().sharedMaterials[0].renderQueue = 3002;
                }
                else
                    GetComponent<MeshRenderer>().sharedMaterials[0].renderQueue = 3001;
            }
        }
        if (!ctrl)
            GetComponent<MeshRenderer>().sharedMaterials[0].renderQueue = 3001;

    }
}
