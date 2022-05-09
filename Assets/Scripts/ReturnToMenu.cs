using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToMenu : MonoBehaviour
{
    [SerializeField] private string sceneName;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            LoadScene();
    }

    private void LoadScene()
    {
        StartCoroutine(GameObject.FindObjectOfType<SceneFader>().FadeAndLoadScene(SceneFader.FadeDirection.In, sceneName));
    }
}
