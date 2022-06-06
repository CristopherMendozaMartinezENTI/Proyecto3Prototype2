using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogTrigger : MonoBehaviour
{
    [SerializeField] private int dialogLenght;
    private GameObject UI_Subtitles;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            gameObject.GetComponent<AudioSource>().enabled = true;
            UI_Subtitles = GameObject.Find("HUD").gameObject.transform.Find("Subtitles").gameObject;
            StartCoroutine(setSubtitles());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            gameObject.GetComponent<BoxCollider>().enabled = false;
    }

    private void Update()
    {
        if (Time.timeScale == 0)
            gameObject.GetComponent<AudioSource>().Pause();
        else if (Time.timeScale != 0 && !gameObject.GetComponent<AudioSource>().isPlaying)
            gameObject.GetComponent<AudioSource>().Play();
    }

    private IEnumerator setSubtitles()
    {
        float clipLeght = 0;
        for (int i = 0; i < dialogLenght; i++)
        {
                yield return new WaitForSeconds(0.5f);
                UI_Subtitles.SetActive(true);
                for (int j = 0; j < DialogManager.sortedList.Count; j++)
                {
                    if (DialogManager.sortedList[j].name == gameObject.name)
                    {
                        UI_Subtitles.GetComponent<Text>().text = DialogManager.staticTriggersList[j].Conversation[i].Dialog;
                        gameObject.GetComponent<AudioSource>().clip = DialogManager.staticTriggersList[j].Conversation[i].Audio;
                        clipLeght = gameObject.GetComponent<AudioSource>().clip.length;
                        yield return new WaitForSeconds(clipLeght - 0.4f);
                        break;
                    }
                }                
        }
        UI_Subtitles.SetActive(false);
        gameObject.SetActive(false);
    }
}
