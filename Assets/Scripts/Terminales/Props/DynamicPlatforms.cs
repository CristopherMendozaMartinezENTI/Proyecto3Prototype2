using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicPlatforms : MonoBehaviour {

    [Range(0, 25)]
    public float speed = 3;

    private Vector3 initialPosition;
    private Vector3 finalPosition;
    private Vector3 destino;
    public Vector3 recorrido;

    public bool showGizmos = true;

    [SerializeField]
    private Color32 colorConnection = new Color32(234,0,217,255);

    void Start()
    {
        initialPosition = transform.position;
        finalPosition = initialPosition + recorrido;
        destino = initialPosition;
        InitGuide();
    }

    void Active()
    {
        destino = finalPosition;
        foreach (TerminalPlatformLight light in GameObject.FindObjectsOfType<TerminalPlatformLight>())
        {
            light.ResetLight();
        }
    }

    void Desactive()
    {
        destino = initialPosition;
        foreach (TerminalPlatformLight light in GameObject.FindObjectsOfType<TerminalPlatformLight>())
        {
            light.ResetLight();
        }
    }
    
    void InitGuide()
    {
        Material material = new Material(Shader.Find("Light"));
        material.SetColor("_emision", colorConnection);
        material.SetColor("_color", colorConnection);
        material.SetFloat("_intensity", 0.8f);
        material.renderQueue = 3000;
        GameObject lineGuide = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Destroy(lineGuide.GetComponent<BoxCollider>());
        lineGuide.transform.position = initialPosition;
        lineGuide.transform.localScale = Vector3.one * 0.1f;
        lineGuide.GetComponent<MeshRenderer>().material = material;
        LineRenderer lR = lineGuide.AddComponent<LineRenderer>();
        lR.startWidth = lR.endWidth = 0.02f;
        lR.SetPosition(0, initialPosition);
        lR.SetPosition(1, finalPosition);
        lR.material = material;

        GameObject endGuide = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Destroy(endGuide.GetComponent<BoxCollider>());
        endGuide.transform.position = finalPosition;
        endGuide.transform.localScale = Vector3.one * 0.1f;
        endGuide.transform.parent = lineGuide.transform;
        endGuide.GetComponent<MeshRenderer>().material = material;
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, destino) > 0.01f)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.up, out hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject.tag == "Pawns")
                {
                    hit.collider.gameObject.transform.parent = transform;
                }
            }
            else if (transform.childCount != 0 && GameObject.Find("PawnsContainer"))
            {
                transform.GetChild(0).transform.parent = GameObject.Find("PawnsContainer").transform;
            }

            transform.position = Vector3.Lerp(transform.position, destino, speed * Time.deltaTime);
        }
        else if(transform.position != destino)
        {
            transform.position = destino;

            if (transform.childCount != 0 && GameObject.Find("PawnsContainer"))
            {
                transform.GetChild(0).transform.parent = GameObject.Find("PawnsContainer").transform;
            }

            GameObject.FindObjectOfType<TerminalGameManager>().UpdateScene();
        }
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        if (showGizmos && recorrido != Vector3.zero)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position + recorrido, GetComponent<BoxCollider>().size);
            Gizmos.DrawWireCube(transform.position, GetComponent<BoxCollider>().size);
            Gizmos.DrawLine(transform.position, transform.position + recorrido);
            Gizmos.color = new Color(1,0,0,0.5f);
            Gizmos.DrawCube(transform.position + recorrido, GetComponent<BoxCollider>().size);
        }
    }
#endif
}
