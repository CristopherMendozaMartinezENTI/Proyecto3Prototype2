using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private GameObject playCamera;
    [SerializeField] private GameObject optionsCamera;
    [SerializeField] private GameObject creditsCamera;
    [SerializeField] private GameObject exitCamera;
    [SerializeField] private GameObject returnButton;
    [SerializeField] private GameObject SettingsCanvas;
    [SerializeField] private GameObject exitCanvas;
    [SerializeField] List<GameObject> uiElements;

    private CheckPointSystem cps;
    [SerializeField] private List<string> sceneNames;

    private void Start()
    {
        cps = GameObject.FindGameObjectWithTag("CPS").GetComponent<CheckPointSystem>();
        sceneNames.Add("SC-1 TMP");
        sceneNames.Add("SC-2");
        sceneNames.Add("SC - Final");
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && playCamera.activeInHierarchy == false)
        {
            if (optionsCamera.activeInHierarchy || creditsCamera.activeInHierarchy || exitCamera.activeInHierarchy) enableMainCamera();
            else enableExitCamera();
        }
    }


    private void LoadScene()
    {
        StartCoroutine(GameObject.FindObjectOfType<SceneFader>().FadeAndLoadScene(SceneFader.FadeDirection.In, sceneName));
    }

    public void startGame()
    {
        DataPersistenceManager.instance.NewGame();
        StartCoroutine(enablePlayCamera());
    }

    public void loadGame()
    {
        DataPersistenceManager.instance.LoadGame();
        cps.loadOrReset = true;
        sceneName = sceneNames[cps.currentLevel];
        StartCoroutine(enablePlayCamera());
    }

    IEnumerator enablePlayCamera()
    {
        foreach(GameObject uiElement in uiElements)
        {
            uiElement.SetActive(false);
        }
        playCamera.SetActive(true);
        yield return new WaitForSeconds(1);
        LoadScene();
    }

    public void enableOptionsCamera()
    {
        foreach (GameObject uiElement in uiElements)
        {
            uiElement.SetActive(false);
        }
        optionsCamera.SetActive(true);
        SettingsCanvas.SetActive(true);
        returnButton.SetActive(true);
    }

    public void enableCreditsCamera()
    {
        foreach (GameObject uiElement in uiElements)
        {
            uiElement.SetActive(false);
        }
        creditsCamera.SetActive(true);
        returnButton.SetActive(true);
    }

    public void enableMainCamera()
    {
        foreach (GameObject uiElement in uiElements)
        {
            uiElement.SetActive(true);
        }
        optionsCamera.SetActive(false);
        SettingsCanvas.SetActive(false);
        creditsCamera.SetActive(false);
        returnButton.SetActive(false);
        exitCanvas.SetActive(false);
        exitCamera.SetActive(false);
    }

    public void enableExitCamera()
    {
        foreach (GameObject uiElement in uiElements)
        {
            uiElement.SetActive(false);
        }
        exitCamera.SetActive(true);
        exitCanvas.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
