using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRiggChange : MonoBehaviour {

    public GameObject cameraRigg;
    [SerializeField]
    private int startCamera = 0;
    [SerializeField]
    int offsetY = 0;
    [SerializeField]
    [Range(1.0f,10.0f)]
    private float startZoom = 5.0f;

    public void InitScene(TerminalGameManager gM)
    {
        GameObject newRigg = GameObject.Instantiate(cameraRigg);
        newRigg.transform.parent = gM.transform;
        newRigg.GetComponent<CameraRigg>().GetGameManager = gM;
        newRigg.GetComponent<CameraRigg>().StartPosition(offsetY);
        newRigg.GetComponent<CameraRigg>().CurrentPos = Mathf.Clamp(startCamera -1, 0, 7);
        newRigg.GetComponentInChildren<Camera>().orthographicSize = startZoom;
        gM.MainCam = newRigg.GetComponentInChildren<Camera>().gameObject;
        Destroy(gameObject);
    }
}
