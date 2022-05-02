using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject PauseMenuCanvas;
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
        }
    }
    public void newResetScene()
    {
        StartCoroutine(ResetScene());
    }
    public IEnumerator ResetScene()
    {
;
        gameIsPaused = false;
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        yield return new WaitForSeconds(1);
        PauseMenuCanvas.SetActive(false);
        StartCoroutine(GameObject.FindObjectOfType<SceneFader>().FadeAndLoadScene(SceneFader.FadeDirection.In, SceneManager.GetActiveScene().name));
    }
    public void newExitScene()
    {
        StartCoroutine(ExitScene());
    }
    public IEnumerator ExitScene()
    {
        gameIsPaused = false;
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        yield return new WaitForSeconds(1);
        PauseMenuCanvas.SetActive(false);
        StartCoroutine(GameObject.FindObjectOfType<SceneFader>().FadeAndLoadScene(SceneFader.FadeDirection.In, "MainMenu"));
    }

}
