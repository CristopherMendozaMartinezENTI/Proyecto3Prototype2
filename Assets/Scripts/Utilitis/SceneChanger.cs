
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    CheckPointSystem cps;
    private void Start()
    {
        cps = GameObject.FindGameObjectWithTag("CPS").GetComponent<CheckPointSystem>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            cps.lastCheckPoint = cps.startingPositions[0];
            cps.lastRotation = Quaternion.Euler(cps.startingRotations[0]);
            SceneManager.LoadScene(1);
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            cps.lastCheckPoint = cps.startingPositions[1];
            cps.lastRotation = Quaternion.Euler(cps.startingRotations[1]);
            SceneManager.LoadScene(2);
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            cps.lastCheckPoint = cps.startingPositions[2];
            cps.lastRotation = Quaternion.Euler(cps.startingRotations[2]);

            SceneManager.LoadScene(3);
        }
/*
        if (Input.GetKeyDown(KeyCode.F4))
        {
            SceneManager.LoadScene(0);
        }

        if (Input.GetKeyDown(KeyCode.F5))
        {
            cps.lastCheckPoint = cps.startingPositions[0];
            cps.lastRotation = Quaternion.Euler(cps.startingRotations[0]);
            SceneManager.LoadScene(5);
        }

        if (Input.GetKeyDown(KeyCode.F6))
        {
            cps.lastCheckPoint = cps.startingPositions[0]; 
            cps.lastRotation = Quaternion.Euler(cps.startingRotations[0]);
            SceneManager.LoadScene(6);
        }

        if (Input.GetKeyDown(KeyCode.F7))
        {
            cps.lastCheckPoint = cps.startingPositions[0];
            cps.lastRotation = Quaternion.Euler(cps.startingRotations[0]);
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
            SceneManager.LoadScene(0);
        }

        if (Input.GetKeyDown(KeyCode.F11))
        {
            SceneManager.LoadScene(11);
        }
*/
    }
}
