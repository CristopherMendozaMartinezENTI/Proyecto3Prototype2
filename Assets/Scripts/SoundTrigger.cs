using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundTrigger : MonoBehaviour
{
    GameObject UI_Subtitles;
    public string DialogText = "DefaultText";

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            gameObject.GetComponent<AudioSource>().enabled = true;

            UI_Subtitles = GameObject.Find("HUD").gameObject.transform.Find("Subtitles").gameObject;
            StartCoroutine(setSubtitles(gameObject.GetComponent<AudioSource>().clip.length, DialogText));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }

    private void Update()
    {
        if (Time.timeScale == 0)
            gameObject.GetComponent<AudioSource>().Pause();
        else if (Time.timeScale != 0 && !gameObject.GetComponent<AudioSource>().isPlaying)
            gameObject.GetComponent<AudioSource>().Play();
    }

    private IEnumerator setSubtitles(float sec, string text)
    {
        yield return new WaitForSeconds(0.7f);
        
        UI_Subtitles.SetActive(true);
        UI_Subtitles.GetComponent<Text>().text = text;

        yield return new WaitForSeconds(sec - 0.3f);
        
        UI_Subtitles.SetActive(false);
        Destroy(gameObject);
    }


}
