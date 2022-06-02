using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPointSystem : MonoBehaviour
{
    private static CheckPointSystem instance;
    public Vector3 lastCheckPoint;
    public Quaternion lastRotation;

    private Scene currentScene, lastScene;
    private string sceneName;

    private Vector3 startingPosition = new Vector3(-111.8813f, -3.5f, -0.9537506f);
    private Vector3 startingPosition1 = new Vector3(12.00013f, 0.02685702f, 8.397099f);
    private Vector3 startingPosition2 = new Vector3(30.09577f, 34.10477f, 19.0097f);
    private Vector3 startingPosition3 = new Vector3(-111.2674f, 16.27318f, 96.5537f);

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else Destroy(gameObject);

        lastScene = SceneManager.GetActiveScene();
    }

    private void Update()
    {
        currentScene = SceneManager.GetActiveScene();
        if (currentScene != lastScene)
        {
            sceneName = SceneManager.GetActiveScene().name;
            LoadNewLevel(sceneName);
            lastScene = currentScene;
        }
    }


    private void LoadNewLevel(string name)
    {
        switch (name)
        {
            case "SC-1":
                lastCheckPoint = startingPosition1;
                break;
            case "SC-2":
                lastCheckPoint = startingPosition2;
                break;
            case "SC-3":
                lastCheckPoint = startingPosition3;
                break;
            case "":
                lastCheckPoint = startingPosition;
                break;
        }
    }
}
