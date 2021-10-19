using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRigg : MonoBehaviour {

    private List<Transform> cameraGuides;
    private int currentPos;

    private TerminalGameManager myGameManager;

    public TerminalGameManager GetGameManager
    {
        set { myGameManager = value; }
    }

    private bool movingCamera = false;

    public bool IsMoving
    {
        get { return movingCamera; }
    }
    public int CurrentPos
    {
        get { return currentPos; }
        set { currentPos = value; }
    }

    // Use this for initialization
    void Start()
    {
        movingCamera = false;
        cameraGuides = new List<Transform>(GetComponentsInChildren<Transform>());
        Transform toRemove = null;
        foreach (Transform child in cameraGuides)
        {
            if (child == gameObject.transform)
                toRemove = child;
            else
            {
                child.localPosition = child.TransformDirection(Vector3.back * 20);
            }
        }

        cameraGuides.Remove(toRemove);
        myGameManager.SelPlayer = null;

        StartPosition();
    }
    
    public void SetZoom(float zoom)
    {
        transform.GetComponentInChildren<Camera>().orthographicSize = zoom;
    }

    void StartPosition()
    {
        Platforms[] platforms = myGameManager.transform.Find("Platfomrs").transform.GetComponentsInChildren<Platforms>();

        Vector2[] minMaxPositions = new Vector2[3];
        if (platforms.Length > 0)
        {
            minMaxPositions[0].x = minMaxPositions[0].y = platforms[0].transform.position.x;
            minMaxPositions[1].x = minMaxPositions[1].y = platforms[0].transform.position.y;
            minMaxPositions[2].x = minMaxPositions[2].y = platforms[0].transform.position.z;

            foreach (Platforms child in platforms)
            {
                if (child.transform.position.x < minMaxPositions[0].x)
                {
                    minMaxPositions[0].x = child.transform.position.x;
                }
                else if (child.transform.position.x > minMaxPositions[0].y)
                {
                    minMaxPositions[0].y = child.transform.position.x;
                }

                if (child.transform.position.y < minMaxPositions[1].x)
                {
                    minMaxPositions[1].x = child.transform.position.y;
                }
                else if (child.transform.position.y > minMaxPositions[1].y)
                {
                    minMaxPositions[1].y = child.transform.position.y;
                }

                if (child.transform.position.z < minMaxPositions[2].x)
                {
                    minMaxPositions[2].x = child.transform.position.z;
                }
                else if (child.transform.position.z > minMaxPositions[2].y)
                {
                    minMaxPositions[2].y = child.transform.position.z;
                }
            }
        }

        transform.position = new Vector3(
            /*x*/ (minMaxPositions[0].y - ((minMaxPositions[0].y - minMaxPositions[0].x) / 2)),
            /*y*/ (minMaxPositions[1].y - ((minMaxPositions[1].y - minMaxPositions[1].x) / 2)),
            /*z*/ (minMaxPositions[2].y - ((minMaxPositions[2].y - minMaxPositions[2].x) / 2)));

    }
	// Update is called once per frame
	void LateUpdate ()
    {
        MoveCamera();
        //FollowPlayers();
    }

    private void MoveCamera()
    {
        if (Vector3.Distance(myGameManager.MainCam.transform.position, cameraGuides[currentPos].transform.position) > 0.1f)
        {
            myGameManager.MainCam.transform.position = Vector3.Slerp(myGameManager.MainCam.transform.position, cameraGuides[currentPos].transform.position, 8f * Time.deltaTime);
            myGameManager.MainCam.transform.rotation = Quaternion.Lerp(myGameManager.MainCam.transform.rotation, cameraGuides[currentPos].transform.rotation, 8f * Time.deltaTime);
            if (!movingCamera) movingCamera = true;
        }
        else if (movingCamera)
        {
            
            
            myGameManager.MainCam.transform.position = cameraGuides[currentPos].transform.position;
            myGameManager.MainCam.transform.rotation = cameraGuides[currentPos].transform.rotation;
            movingCamera = false;
        }
    }

    

    public void ChangeCamera(char direction)
    {
        if (direction == 'R')
        {
            if (currentPos == cameraGuides.Count - 1)
                currentPos = 0;
            else
                currentPos++;
        }
        else if (direction == 'L')
        {
            if (currentPos == 0)
                currentPos = cameraGuides.Count - 1;
            else
                currentPos--;
        }
    }
}


//private void FollowPlayers()
//{
//    Vector3 newPos = Vector3.zero;
//    if (myGameManager.SelPlayer == null && myGameManager.PawnsList.Count > 1)
//    {
//        Vector3 minPos, maxPos;
//        minPos = maxPos = myGameManager.PawnsList[0].transform.position;

//        for (int i = 1; i < myGameManager.PawnsList.Count; i++)
//        {
//            if (myGameManager.PawnsList[i].transform.position.x < minPos.x)
//                minPos.x = myGameManager.PawnsList[i].transform.position.x;
//            else if (myGameManager.PawnsList[i].transform.position.x > maxPos.x)
//                maxPos.x = myGameManager.PawnsList[i].transform.position.x;

//            if (myGameManager.PawnsList[i].transform.position.y < minPos.y)
//                minPos.y = myGameManager.PawnsList[i].transform.position.y;
//            else if (myGameManager.PawnsList[i].transform.position.x > maxPos.y)
//                maxPos.y = myGameManager.PawnsList[i].transform.position.y;

//            if (myGameManager.PawnsList[i].transform.position.z < minPos.z)
//                minPos.z = myGameManager.PawnsList[i].transform.position.z;
//            else if (myGameManager.PawnsList[i].transform.position.z > maxPos.z)
//                maxPos.z = myGameManager.PawnsList[i].transform.position.z;
//        }

//        newPos = (minPos - maxPos) / 2;
//        newPos += maxPos;            
//    }
//    else
//    {
//       if (myGameManager.SelPlayer) newPos = myGameManager.SelPlayer.transform.position;
//       else newPos = myGameManager.PawnsList[0].transform.position;
//    }

//    if (Vector3.Distance(transform.position, newPos) > 0.1f)
//    {
//        transform.position = Vector3.Lerp(transform.position, newPos, 2.0f * Time.deltaTime);
//    }

//}

