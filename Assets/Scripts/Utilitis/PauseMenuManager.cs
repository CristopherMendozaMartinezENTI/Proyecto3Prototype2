using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject PauseMenuCanvas;
    [SerializeField] private GameObject SettingsCanvas;
    [SerializeField] List<GameObject> uiElements;
    private static bool gameIsPaused;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameIsPaused = !gameIsPaused;
            PauseGame();
        }
    }

    private void PauseGame()
    {
        if (gameIsPaused)
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            PauseMenuCanvas.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            PauseMenuCanvas.SetActive(false);
            SettingsCanvas.SetActive(false);
        }
    }

    public void ResetScene()
    {
        gameIsPaused = false;
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PauseMenuCanvas.SetActive(false);
        StartCoroutine(GameObject.FindObjectOfType<SceneFader>().FadeAndLoadScene(SceneFader.FadeDirection.In, SceneManager.GetActiveScene().name));
    }

    public void enableOptions()
    {
        foreach (GameObject uiElement in uiElements)
        {
            uiElement.SetActive(false);
        }
        SettingsCanvas.SetActive(true);
    }

    public void SaveSettingsButton()
    {
        PauseMenuCanvas.SetActive(true);
        SettingsCanvas.SetActive(false);
    }

    public void ExitScene()
    {
        gameIsPaused = false;
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PauseMenuCanvas.SetActive(false);
        StartCoroutine(GameObject.FindObjectOfType<SceneFader>().FadeAndLoadScene(SceneFader.FadeDirection.In, "MainMenu"));
    }
}
