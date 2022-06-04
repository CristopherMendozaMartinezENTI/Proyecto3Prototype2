using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundTrigger : MonoBehaviour
{
    GameObject UI_Subtitles;
    public string DialogText = "DefaultText";

    private List<Component> listComp = new List<Component>();
    private Component[] components;

    private void Start()
    {
        //Miro si tiene varios audios y los pongo en una lista
        components = gameObject.GetComponents(typeof(Component));

        for (int i = 0; i < components.Length; i++)
        {
            if (components[i].GetType().ToString() == "UnityEngine.AudioSource")
                listComp.Add(components[i]);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            gameObject.GetComponent<AudioSource>().enabled = true;

            UI_Subtitles = GameObject.Find("HUD").gameObject.transform.Find("Subtitles").gameObject;

            StartCoroutine(setSubtitles(DialogText));
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

    private IEnumerator setSubtitles(string text)
    {

        /*if (listComp.Count > 1)
        {
            for (int i = 0; i < listComp.Count; i++)
            {
                yield return new WaitForSeconds(0.7f);

                UI_Subtitles.SetActive(true);

                for (int j = 0; j < AudioTrigger.listGO.Count; j++)
                {
                    if (AudioTrigger.listGO[j].name == gameObject.name)
                    {
                        UI_Subtitles.GetComponent<Text>().text = AudioTrigger.triggersList[j].Conversation[i].Dialog;
                    }
                }

                yield return new WaitForSeconds(sec - 0.3f);
            }

            UI_Subtitles.SetActive(false);
            Destroy(gameObject);
        }
        else 
        {
            
            yield return new WaitForSeconds(0.7f);

            UI_Subtitles.SetActive(true);
            UI_Subtitles.GetComponent<Text>().text = text;

            yield return new WaitForSeconds(sec - 0.3f);

            UI_Subtitles.SetActive(false);
            Destroy(gameObject);

        }*/

        float sec = 0;

        for (int i = 0; i < listComp.Count; i++)
            {
                yield return new WaitForSeconds(0.7f);

                UI_Subtitles.SetActive(true);

                for (int j = 0; j < AudioTrigger.listGO.Count; j++)
                {
                    if (AudioTrigger.listGO[j].name == gameObject.name)
                    {
                        UI_Subtitles.GetComponent<Text>().text = AudioTrigger.triggersList[j].Conversation[i].Dialog;
                        gameObject.GetComponent<AudioSource>().clip = AudioTrigger.triggersList[j].Conversation[i].Audio;
                        sec = gameObject.GetComponent<AudioSource>().clip.length;

                        yield return new WaitForSeconds(sec - 1.0f);
                    }
                }

                
            }

            UI_Subtitles.SetActive(false);
            Destroy(gameObject);
    }

}
