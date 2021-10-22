using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct SwitchNodes
{
    public GameObject link;
    public List<GameObject> conNode;
    public List<LineRenderer> conLine;
}
public class Switch : MonoBehaviour {

    [SerializeField]
    private enum EnumSwitchType { PULSE, BOTTON , SWITCH};
    [SerializeField]
    private EnumSwitchType switchType = EnumSwitchType.PULSE;

    bool pressed = false;
    public List<GameObject> links;
    private List<SwitchNodes> nodes = new List<SwitchNodes>();
    Collider[] hits;
    private int lastHits = 0;
    private GameObject conContainer;
    const float nodeDistance = -1.5f;
    const float nodeSize = 0.1f;

    public LayerMask layer;
    [SerializeField]
    private Color32 connectionsColor = new Color32(19,189,198,255);
    
    private void Start()
    {
        InitConnections();
    }

    void Update ()
    {
        switch (switchType)
        {
            case EnumSwitchType.PULSE:

                hits = Physics.OverlapBox(transform.position + Vector3.up * 1.2f, Vector3.one * 0.4f,new Quaternion(0,0,0,0), layer);

                if (hits.Length > 0 && !pressed)
                {
                    foreach (GameObject link in links)
                    {
                        link.SendMessage("Active", SendMessageOptions.DontRequireReceiver);
                    }
                    pressed = true;
                }
                else if (hits.Length == 0 && pressed)
                {
                    foreach (GameObject link in links)
                    {
                        link.SendMessage("Desactive", SendMessageOptions.DontRequireReceiver);
                    }
                    pressed = false;
                }
                break;
            case EnumSwitchType.BOTTON:
                
                if (!pressed)
                {
                    hits = Physics.OverlapBox(transform.position + Vector3.up * 1.2f, Vector3.one * 0.4f);
                    if (hits.Length > 0)
                    {
                        foreach (GameObject link in links)
                        {
                            link.SendMessage("Active", SendMessageOptions.DontRequireReceiver);
                        }
                        pressed = true;
                    }
                }
                break;
            case EnumSwitchType.SWITCH:
                hits = Physics.OverlapBox(transform.position + Vector3.up * 1.2f, Vector3.one * 0.4f);

                if (lastHits == 0 && hits.Length != 0)
                {
                    if (!pressed)
                    {
                        foreach (GameObject link in links)
                        {
                            link.SendMessage("Active", SendMessageOptions.DontRequireReceiver);
                        }
                        pressed = true;
                    }
                    else if (pressed)
                    {
                        foreach (GameObject link in links)
                        {
                            link.SendMessage("Desactive", SendMessageOptions.DontRequireReceiver);
                        }
                        pressed = false;
                    }
                }

                lastHits = hits.Length;
                break;
                
        }
        
        
        
    }

    private void InitConnections()
    {
        Material rayMat = new Material(Shader.Find("Light"));
        rayMat.SetColor("_emision", connectionsColor);
        rayMat.SetColor("_color", connectionsColor);
        rayMat.SetFloat("_intensity", 0.8f);
        rayMat.renderQueue = 3000;
        GameObject objectTemp = GameObject.CreatePrimitive(PrimitiveType.Cube);
        if (objectTemp.GetComponent<BoxCollider>()) DestroyImmediate(objectTemp.GetComponent<BoxCollider>());
        objectTemp.GetComponent<MeshRenderer>().material = rayMat;
        objectTemp.name = transform.name + "_Container";
        objectTemp.transform.position = transform.position + Vector3.up * nodeDistance;
        objectTemp.transform.localScale = Vector3.one * nodeSize;
        conContainer = objectTemp;
        LineRenderer lTemp = conContainer.AddComponent<LineRenderer>();
        lTemp.SetPosition(0, transform.position +Vector3.up *0.49f);
        lTemp.SetPosition(1, transform.position + Vector3.up * nodeDistance);
        lTemp.startWidth = lTemp.endWidth = nodeSize * .2f;
        lTemp.material = rayMat;


        foreach (GameObject link in links)
        {
            Vector3 Path = (link.transform.position - (transform.position + Vector3.up * nodeDistance));
            Vector3 currentPos = transform.position + Vector3.up * nodeDistance;
            SwitchNodes currentNode = new SwitchNodes { link = link, conLine = new List<LineRenderer>(), conNode = new List<GameObject>() };

            rayMat.color = connectionsColor;
            if (Path.x != 0)
            {
                nodes.Add(currentNode);
                objectTemp = GameObject.CreatePrimitive(PrimitiveType.Cube);
                if (objectTemp.GetComponent<BoxCollider>()) DestroyImmediate(objectTemp.GetComponent<BoxCollider>());
                currentNode.conNode.Add(objectTemp);
                objectTemp.transform.localScale = Vector3.one * nodeSize;
                objectTemp.GetComponent<MeshRenderer>().material = rayMat;
                objectTemp.transform.parent = conContainer.transform;
                nodes[nodes.Count - 1].conNode.Add(objectTemp);
                lTemp = objectTemp.AddComponent<LineRenderer>();
                currentNode.conLine.Add(lTemp);
                lTemp.startWidth = lTemp.endWidth = nodeSize * .2f;
                lTemp.SetPosition(0, currentPos);
                currentPos += Vector3.right * Path.x;
                objectTemp.transform.position = currentPos;
                lTemp.SetPosition(1, currentPos);
                lTemp.material = rayMat;
            }
            if (Path.y != 0)
            {
                nodes.Add(currentNode);
                objectTemp = GameObject.CreatePrimitive(PrimitiveType.Cube);
                if (objectTemp.GetComponent<BoxCollider>()) DestroyImmediate(objectTemp.GetComponent<BoxCollider>());
                objectTemp.transform.localScale = Vector3.one * nodeSize;
                objectTemp.GetComponent<MeshRenderer>().material = rayMat;
                objectTemp.transform.parent = conContainer.transform;
                nodes[nodes.Count - 1].conNode.Add(objectTemp);
                lTemp = objectTemp.AddComponent<LineRenderer>();
                currentNode.conLine.Add(lTemp);
                lTemp.startWidth = lTemp.endWidth = nodeSize * .2f;
                lTemp.SetPosition(0, currentPos);
                currentPos += Vector3.up * Path.y;
                objectTemp.transform.position = currentPos;
                lTemp.SetPosition(1, currentPos);
                lTemp.material = rayMat;
            }
            if (Path.z != 0)
            {
                nodes.Add(currentNode);
                objectTemp = new GameObject();
                objectTemp.transform.localScale = Vector3.one * nodeSize;
                objectTemp.transform.parent = conContainer.transform;
                nodes[nodes.Count - 1].conNode.Add(objectTemp);
                lTemp = objectTemp.AddComponent<LineRenderer>();
                currentNode.conLine.Add(lTemp);
                lTemp.startWidth = lTemp.endWidth = nodeSize * .2f;
                lTemp.SetPosition(0, currentPos);
                currentPos += Vector3.forward * Path.z;
                objectTemp.transform.position = currentPos;
                lTemp.SetPosition(1, currentPos);
                lTemp.material = rayMat;
            }

        }
    }

    #region GIZMOS
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.up * 1.5f);

            if (links.Count == 0)
            {
                Gizmos.color = new Color(1, 0, 0, 0.5f);
                Gizmos.DrawCube(transform.position + Vector3.up * 1.5f, Vector3.one * 0.5f);
            }
            else
            {
                Gizmos.DrawCube(transform.position + Vector3.up * 1.5f, Vector3.one * 0.1f);

                for (int i = 0; i < links.Count; i++)
                {
                    Vector3 Path = (links[i].transform.position - (transform.position + Vector3.up * 1.5f));
                    Vector3 currentPos = transform.position + Vector3.up * 1.5f;
                    if (Path.x != 0)
                    {
                        Gizmos.DrawLine(currentPos, currentPos + new Vector3(Path.x, 0, 0));
                        currentPos += new Vector3(Path.x, 0, 0);
                        Gizmos.DrawCube(currentPos, Vector3.one * 0.1f);
                    }
                    if (Path.y != 0)
                    {
                        Gizmos.DrawLine(currentPos, currentPos + new Vector3(0, Path.y, 0));
                        currentPos += new Vector3(0, Path.y, 0);
                        Gizmos.DrawCube(currentPos, Vector3.one * 0.1f);
                    }
                    if (Path.z != 0)
                    {
                        Gizmos.DrawLine(currentPos, currentPos + new Vector3(0, 0, Path.z));
                        currentPos += new Vector3(0, 0, Path.z);
                        Gizmos.DrawCube(currentPos, Vector3.one * 0.1f);
                    }
                }
            }
        }
    }

#endif
#endregion

}
