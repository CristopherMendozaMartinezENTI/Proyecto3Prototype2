using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRewind : MonoBehaviour
{
    [Header("Rewind Options")]
    [SerializeField] private int maxRewindData = 30;
    [SerializeField] private float secondsBetweenData = 0.5f;
    [SerializeField] private float rewindDuration = 1.25f;
    [SerializeField] private CameraController playerCameraController;
    private bool canCollectRecallData = true;
    private float currentDataTimer = 0.0f;
    AudioSource[] audiosSources;
    PropPhysicsController[] props;

    void Awake()
    {
        audiosSources = FindObjectsOfType<AudioSource>();
        props = FindObjectsOfType<PropPhysicsController>();
    }

    [System.Serializable]
    private class PlayerRewindData
    {
        public Vector3 playerPos;
        public Quaternion playerRot;
        public Quaternion cameraRot;
    }

    [System.Serializable]
    private class PropsRewindData
    {
        public Vector3 objPos;
        public Quaternion objRot;
    }

    [SerializeField] private List<PlayerRewindData> rData = new List<PlayerRewindData>();
    [SerializeField] private List<List<PropsRewindData>> rPropsData = new List<List<PropsRewindData>>();

    private void Update()
    {
        manageRewindData();

        for (int i = 0; i < rData.Count - 1; i++)
        {
            Debug.DrawLine(rData[i].playerPos, rData[i + 1].playerPos);
        }

        getRewindInpunt();
    }

    private void getRewindInpunt()
    {
        if (Input.GetKeyUp(KeyCode.R) && rData.Count != 0)
        {
            StartCoroutine(Rewind());
        }
    }

    private void manageRewindData()
    {
        if(Input.GetKey(KeyCode.R))
        {
            currentDataTimer += Time.deltaTime;
            if (canCollectRecallData)
            {
                if (currentDataTimer > secondsBetweenData)
                {
                    if (rData.Count >= maxRewindData)
                    {
                        rData.RemoveAt(0);
                        rPropsData.RemoveAt(0);
                    }
                    rData.Add(GetPlayerRewindData());
                    rPropsData.Add(GetPropRewindData());
                    currentDataTimer = 0.0f;
                }
            }
        }
    }

    private PlayerRewindData GetPlayerRewindData()
    {
        return new PlayerRewindData()
        {
            playerPos = transform.position,
            playerRot = transform.rotation,
            cameraRot = playerCameraController.transform.rotation
        };
    }

    private List<PropsRewindData> GetPropRewindData()
    {
        List<PropsRewindData> objData = new List<PropsRewindData>();

        for (int i = 0; i < props.Length; i++)
        {
            Vector3 pos = props[i].gameObject.transform.position;
            Quaternion rot = props[i].gameObject.transform.rotation;
            PropsRewindData data  = new PropsRewindData()
            {
                objPos = pos,
                objRot = rot
            };
            objData.Add(data);
        }

        return objData;
    }

    private IEnumerator Rewind()
    {
        playerCameraController.LockRotation(true);
        canCollectRecallData = false;
        float secordsPerData = rewindDuration / rData.Count;
        Vector3 curretDataPlayerStarPos = transform.position;
        Quaternion currentPlayerStartRotation = transform.rotation;
        Quaternion currentCameraStartRotation = playerCameraController.transform.rotation;

        List<Vector3> curretDataPropStarPos = new List<Vector3>();
        List<Quaternion> currentPropStartRot = new List<Quaternion>();

        for (int i = 0; i < props.Length; i++)
        {
            curretDataPropStarPos.Add(props[i].gameObject.transform.position);
            currentPropStartRot.Add(props[i].gameObject.transform.rotation);
        }

        for (int i = 0; i < audiosSources.Length; i++)
        {
            audiosSources[i].pitch = -audiosSources[i].pitch;
        }

        Camera.main.GetComponent<ScanlinesEffect>().enabled = true;
        while (rData.Count > 0)
        {
            float t = 0;
            while (t < secordsPerData)
            {
                transform.position = Vector3.Lerp(curretDataPlayerStarPos,
                    rData[rData.Count - 1].playerPos, t / secordsPerData);

                transform.rotation = Quaternion.Lerp(currentPlayerStartRotation,
                         rData[rData.Count - 1].playerRot, t / secordsPerData);

                playerCameraController.transform.rotation = Quaternion.Lerp(currentCameraStartRotation,
                rData[rData.Count - 1].cameraRot, t / secordsPerData);

                for (int i = 0; i < props.Length; i++)
                {
                    props[i].gameObject.transform.position = Vector3.Lerp(curretDataPropStarPos[i],
                                                    rPropsData[rPropsData.Count - 1][i].objPos, t / secordsPerData); ;
                    props[i].gameObject.transform.rotation = Quaternion.Lerp(currentPropStartRot[i],
                                                   rPropsData[rPropsData.Count - 1][i].objRot, t / secordsPerData); ;
                }

                t += Time.deltaTime;

                yield return null;
            }
            curretDataPlayerStarPos = rData[rData.Count - 1].playerPos;
            currentPlayerStartRotation = rData[rData.Count - 1].playerRot;
            currentCameraStartRotation = rData[rData.Count - 1].cameraRot;
            rData.RemoveAt(rData.Count - 1);

            for (int i = 0; i < props.Length; i++)
            {
                curretDataPropStarPos[i] = rPropsData[rPropsData.Count - 1][i].objPos;
                currentPropStartRot[i] = rPropsData[rPropsData.Count - 1][i].objRot;
            }

            rPropsData.RemoveAt(rPropsData.Count - 1);
        }
        playerCameraController.LockRotation(false);
        canCollectRecallData = true;
        Camera.main.GetComponent<ScanlinesEffect>().enabled = false;
        for (int i = 0; i < audiosSources.Length; i++)
        {
            audiosSources[i].pitch = -audiosSources[i].pitch;
        }
    }
}
