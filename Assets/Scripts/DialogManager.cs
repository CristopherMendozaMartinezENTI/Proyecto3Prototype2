using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class DialogManager : MonoBehaviour
{
    [System.Serializable]
    public class AudioDialog
    {
        public string Dialog;
        public AudioClip Audio;
    }

    [System.Serializable]
    public class AudioTrigger
    {
        public GameObject Trigger;
        public AudioDialog[] Conversation;
    }

    [SerializeField] private List<AudioTrigger> triggersList = new List<AudioTrigger>();
    private List<DialogTrigger> sortedList = new List<DialogTrigger>();

    private AudioSource source;
    private GameObject UI_Subtitles;
    private void Start()
    {
        foreach (DialogTrigger gameObj in FindObjectsOfType<DialogTrigger>())
            if (gameObj.name.Contains("Dialog")) sortedList.Add(gameObj);

        sortedList.Sort( delegate (DialogTrigger dialogTriggerFirst, DialogTrigger dialogTriggerSecond)
        {
            return dialogTriggerFirst.name.CompareTo(dialogTriggerSecond.name);
        });

        source = gameObject.GetComponent<AudioSource>();
        UI_Subtitles = GameObject.Find("HUD").gameObject.transform.Find("Subtitles").gameObject;
    }
    private void Update()
    {
        if (Time.timeScale == 0)
            source.Pause();
        else if (Time.timeScale != 0 && !source.isPlaying)
            source.Play();
    }
    public void PlayDialogue(string triggerName)
    {
        StopAllCoroutines();
        StartCoroutine(setSubtitles(triggerName));
    }
    public IEnumerator setSubtitles(string _triggerName)
    {
        float clipLeght = 0;
        AudioTrigger temp = triggersList.Where(obj => obj.Trigger.name == _triggerName).SingleOrDefault();
        DialogTrigger tempDialogTrigger = sortedList.Where(obj => obj.name == _triggerName).SingleOrDefault();

        for (int i = 0; i < temp.Conversation.Length; i++)
        {
            yield return new WaitForSeconds(0.5f);
            UI_Subtitles.SetActive(true);
            for (int j = 0; j < sortedList.Count; j++)
            {
                if (sortedList[j].name == _triggerName)
                {
                    UI_Subtitles.GetComponent<Text>().text = triggersList[j].Conversation[i].Dialog;
                    source.clip = triggersList[j].Conversation[i].Audio;
                    clipLeght = source.clip.length;
                    yield return new WaitForSeconds(clipLeght - 0.4f);
                    break;
                }
            }
        }
        UI_Subtitles.SetActive(false);
        source.clip = null;
    }
    private void OnDestroy()
    {
        triggersList.Clear();
        sortedList.Clear();
    }
}
