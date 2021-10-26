using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalGameManager : MonoBehaviour {
    
    private GameObject selectedPlayer = null;

    public GameObject SelPlayer
    {
        get { return selectedPlayer;}
        set { value = selectedPlayer; }
    }
    
    private List<GameObject> pawns;

    public List<GameObject> PawnsList
    {
        get { return pawns; }
    }

    private GameObject pawnContainer;

    public GameObject PawnContainer
    {
        get { return pawnContainer; }
    }

    private GameObject mainCam;

    public GameObject MainCam
    {
        get { return mainCam; }
        set { mainCam = value;}
    }

    private void Awake()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Application.targetFrameRate = 60;

        pawns = new List<GameObject>();

        foreach (Transform child in transform.GetComponentInChildren<Transform>())
        {
            if (child.GetComponent<CameraRiggChange>())
            {
                child.GetComponent<CameraRiggChange>().InitScene(this);
            }
            else if (child.gameObject.name == "PawnsContainer")
            {
                pawnContainer = child.gameObject;
                foreach(Pawns pawn in pawnContainer.GetComponentsInChildren<Pawns>())
                {
                    pawns.Add(pawn.gameObject);
                    pawn.PawmGm = this;
                }
            }
        }
    }


    private void Update()
    {

        /*Borrar*/
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
        GetInputs();
        GetMouseInputs();
    }

    private void GetInputs()
    {
        
        bool activeInputs = true;
        foreach(GameObject pawn in pawns)
        {
            if (pawn.GetComponent<Pawns>().IsMoving())
                activeInputs = false;
        }

        if(activeInputs)
        {
            CameraRigg rigg = mainCam.transform.parent.gameObject.GetComponent<CameraRigg>();
            if (Input.GetKey(KeyCode.RightArrow) && !rigg.IsMoving)
                rigg.ChangeCamera('R');
            else if (Input.GetKey(KeyCode.LeftArrow) && !rigg.IsMoving)
                rigg.ChangeCamera('L');
        }
    }


    private void GetMouseInputs()
    {
        
        // -------------------------------------------------------- Selected Button 
        if (Input.GetMouseButtonDown(0)&& mainCam)
        {
            RaycastHit hit;
            Ray ray = mainCam.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if (hit.transform != null)
                {
                    if (pawns.Contains(hit.collider.gameObject))
                    {
                        if (selectedPlayer != null && hit.collider.gameObject == selectedPlayer)
                        {
                            selectedPlayer.GetComponent<Outline>().OutlineWidth = 0.0f;
                            selectedPlayer = null;
                        }
                        else
                        {
                            if (selectedPlayer) selectedPlayer.GetComponent<Outline>().OutlineWidth = 0.0f;
                            selectedPlayer = hit.collider.gameObject;
                            selectedPlayer.GetComponent<Outline>().OutlineWidth = 3.5f;
                        }
                    }
                    else if (hit.collider.tag == "Platform" && selectedPlayer)
                    {
                        NewPoint(hit.collider.gameObject);
                    }

                }
            }
        }

        // -------------------------------------------------------- Zoom mouseScrollWheel
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (mainCam.GetComponent<Camera>().orthographicSize > 1f)
                mainCam.GetComponent<Camera>().orthographicSize -= 0.5f;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (mainCam.GetComponent<Camera>().orthographicSize < 10f)
                mainCam.GetComponent<Camera>().orthographicSize += 0.5f;
        }


    }

    public void UpdateScene()
    {
        foreach (Platforms plat in transform.GetComponentsInChildren<Platforms>())
        {
            if (plat.GetComponent<TerminalPlatformLight>())
                plat.UpdateLightsGuides();
        }

        foreach(GameObject pawn in pawns)
        {
            pawn.GetComponent<Pawns>().CheckRenderQueue(mainCam);
        }
    }

    private void NewPoint(GameObject selected)
    {
        selectedPlayer.GetComponent<Pawns>().RepositionCharacter(selected);
    }
}
