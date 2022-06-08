using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPointSystem : MonoBehaviour, IDataPersistence
{
    private static CheckPointSystem instance;
    private Scene currentScene, lastScene;
    
    public bool loadOrReset = false;
    public int currentLevel;
    
    public Vector3 lastCheckPoint;
    public Quaternion lastRotation;


    [HideInInspector] public List<Vector3> startingPositions;
    [HideInInspector] public List<Vector3> startingRotations;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else Destroy(gameObject);

        lastScene = SceneManager.GetActiveScene();

        //Test
        startingPositions.Add(new Vector3(-111.8813f, -3.5f, -0.9537506f));
        startingRotations.Add(new Vector3(0, 90, 0));
        //SC-1
        startingPositions.Add(new Vector3(-33.5f, 0.375f, 12.25005f));
        startingRotations.Add(new Vector3(0, -270, 0));
        //SC-2
        startingPositions.Add(new Vector3(30.09577f, 34.10477f, 19.0097f));
        startingRotations.Add(new Vector3(0, -180, 0));
        //SC - Final
        startingPositions.Add(new Vector3(241.27f, 15.3f, 230.3291f));
        startingRotations.Add(new Vector3(0, 90, 0));

        lastCheckPoint = startingPositions[1];
    }

    private void Update()
    {
        currentScene = SceneManager.GetActiveScene();
        if (currentScene != lastScene)
        {
            if (!loadOrReset)
            {
                LoadNewLevel(currentScene.name);
            }
            loadOrReset = false;
            lastScene = currentScene;
        }
    }
    
    private void LoadNewLevel(string name)
    {
        //Si Reset Level ha sido seleccionado. La posicion del player pasa a ser la posicion inicial del puzle.
        switch (name)
        {
            //SC1
            case "SC-1 TMP":
                lastCheckPoint = startingPositions[1];
                lastRotation = Quaternion.Euler(startingRotations[1]);
                currentLevel = 1;
                break;
            //SC2
            case "SC-2":
                lastCheckPoint = startingPositions[2]; 
                lastRotation = Quaternion.Euler(startingRotations[2]);
                currentLevel = 2;
                break;
            //final
            case "SC - Final":
                lastCheckPoint = startingPositions[3];
                lastRotation = Quaternion.Euler(startingRotations[1]);
                currentLevel = 3;
                break;
            //TEST
            default:
                lastCheckPoint = startingPositions[1];
                lastRotation = Quaternion.Euler(startingRotations[1]);
                currentLevel = 0;
                break;
        }
    }

    
    //Scripts from the Save System (Data Manager)
    public void LoadData(GameData data)
    {
        this.currentLevel = data.currentLevel;
        this.lastCheckPoint = data.lastCheckpoint;
        this.lastRotation = data.lastRotation;
    }
    public void SaveData(GameData data)
    {
        data.currentLevel = this.currentLevel;
        data.lastCheckpoint = this.lastCheckPoint;
        data.lastRotation = this.lastRotation;
    }


}
