using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRewind : MonoBehaviour
{
    [SerializeField] private int maxRewindData = 30;
    [SerializeField] private float secondsBetweenData = 0.5f;
    [SerializeField] private float rewindDuration = 1.25f;
    [SerializeField] private CameraController playerCameraController;
    private bool canCollectRecallData = true;
    private float currentDataTimer = 0.0f;

    [System.Serializable]
    private class RewindData
    {
        public Vector3 playerPos;
        public Quaternion playerRot;
        public Quaternion cameraRot;
    }

    [SerializeField]  private  List<RewindData> rData = new List<RewindData>();

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        manageRewindData();

        for(int i = 0; i < rData.Count - 1; i++)
        {
            Debug.DrawLine(rData[i].playerPos, rData[i + 1].playerPos);
        }

        getRewindInpunt();


    }

    private void getRewindInpunt()
    {
        if(Input.GetKeyDown(KeyCode.E) && rData.Count != 0)
        {
            StartCoroutine(Rewind());
        }
    }

    private void manageRewindData()
    {
        currentDataTimer += Time.deltaTime;
        if (canCollectRecallData)
        {
            if (currentDataTimer > secondsBetweenData)
            {
                if (rData.Count >= maxRewindData)
                {
                    rData.RemoveAt(0);
                }

                rData.Add(GetRewindData());

                currentDataTimer = 0.0f;
            }
        }
    }

    private RewindData GetRewindData()
    {
        return new RewindData()
        {
            playerPos = transform.position,
            playerRot = transform.rotation,
            cameraRot = playerCameraController.transform.rotation
        };
    }

    private IEnumerator Rewind()
    {
        playerCameraController.LockRotation(true);
        canCollectRecallData = false;
        float secordsPerData = rewindDuration / rData.Count;
        Vector3 curretDataPlayerStarPos = transform.position;
        Quaternion currentPlayerStartRotation = transform.rotation;
        //Quaternion currentCameraStartRotation = playerCameraController.transform.rotation;

        while (rData.Count > 0)
        {
            float t = 0;


            while (t < secordsPerData)
            {
                transform.position = Vector3.Lerp(curretDataPlayerStarPos, 
                    rData[rData.Count - 1].playerPos, t /secordsPerData);

                transform.rotation = Quaternion.Lerp(currentPlayerStartRotation,
                         rData[rData.Count - 1].playerRot, t / secordsPerData);

                //transform.rotation = Quaternion.Lerp(currentCameraStartRotation,
                         //rData[rData.Count - 1].cameraRot, t / secordsPerData);

                t += Time.deltaTime;

                yield return null;
            }
            curretDataPlayerStarPos = rData[rData.Count - 1].playerPos;
            currentPlayerStartRotation = rData[rData.Count - 1].playerRot;
            //currentCameraStartRotation = rData[rData.Count - 1].cameraRot;

            rData.RemoveAt(rData.Count - 1);
        }
        playerCameraController.LockRotation(false);
        canCollectRecallData = true;
    }
}
