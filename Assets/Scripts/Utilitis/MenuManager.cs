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
    [SerializeField] private GameObject returnButton;
    [SerializeField] List<GameObject> uiElements;

    private void LoadScene()
    {
        StartCoroutine(GameObject.FindObjectOfType<SceneFader>().FadeAndLoadScene(SceneFader.FadeDirection.In, sceneName));
    }

    public void startGame()
    {
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
        creditsCamera.SetActive(false);
        returnButton.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
