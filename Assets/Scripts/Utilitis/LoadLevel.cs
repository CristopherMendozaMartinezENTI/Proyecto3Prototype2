using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadLevel : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private GameObject pressE;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            pressE.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                StartCoroutine(GameObject.FindObjectOfType<SceneFader>().FadeAndLoadScene(SceneFader.FadeDirection.In, sceneName));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            pressE.SetActive(false);
        }
    }
}
