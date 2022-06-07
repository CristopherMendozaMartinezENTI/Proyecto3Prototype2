using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject PauseMenuCanvas;
    [SerializeField] private GameObject SettingsCanvas;
    [SerializeField] private GameObject saveMessage; 
    [SerializeField] List<GameObject> uiElements;
    private static bool gameIsPaused;
    CheckPointSystem cps;

    private void Start()
    {
        cps = GameObject.FindGameObjectWithTag("CPS").GetComponent<CheckPointSystem>();
    }

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
            saveMessage.SetActive(false);
            SettingsCanvas.SetActive(false);
        }
    }

    public void ResetScene()
    {
        gameIsPaused = false;
        cps.loadOrReset = true;
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PauseMenuCanvas.SetActive(false);
        saveMessage.SetActive(false);
        StartCoroutine(GameObject.FindObjectOfType<SceneFader>().FadeAndLoadScene(SceneFader.FadeDirection.In, SceneManager.GetActiveScene().name));
    }

    public void enableOptions()
    {
        foreach (GameObject uiElement in uiElements)
        {
            uiElement.SetActive(false);
        }
        saveMessage.SetActive(false);
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

    public void SaveGameClicked()
    {
        DataPersistenceManager.instance.SaveGame();
        saveMessage.SetActive(true);
    }

}
