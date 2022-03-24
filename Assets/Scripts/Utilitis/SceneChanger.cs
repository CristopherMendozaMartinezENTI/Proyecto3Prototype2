
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            SceneManager.LoadScene(1);
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            SceneManager.LoadScene(2);
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            SceneManager.LoadScene(3);
        }

        if (Input.GetKeyDown(KeyCode.F4))
        {
            SceneManager.LoadScene(4);
        }

        if (Input.GetKeyDown(KeyCode.F5))
        {
            SceneManager.LoadScene(5);
        }

        if (Input.GetKeyDown(KeyCode.F6))
        {
            SceneManager.LoadScene(6);
        }

        if (Input.GetKeyDown(KeyCode.F7))
        {
            SceneManager.LoadScene(7);
        }

        if (Input.GetKeyDown(KeyCode.F8))
        {
            SceneManager.LoadScene(8);
        }

        if (Input.GetKeyDown(KeyCode.F9))
        {
            SceneManager.LoadScene(9);
        }

        if (Input.GetKeyDown(KeyCode.F10))
        {
            SceneManager.LoadScene(10);
        }

        if (Input.GetKeyDown(KeyCode.F11))
        {
            SceneManager.LoadScene(11);
        }
    }
}
