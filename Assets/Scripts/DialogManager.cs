using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField]
    private List<AudioTrigger> triggersList = new List<AudioTrigger>();
    public static List<AudioTrigger> staticTriggersList = new List<AudioTrigger>();
    public static List<DialogTrigger> sortedList = new List<DialogTrigger>(); 

    private void Start()
    {
        staticTriggersList = triggersList;

        foreach (DialogTrigger gameObj in GameObject.FindObjectsOfType<DialogTrigger>())
            if (gameObj.name.Contains("Dialog")) sortedList.Add(gameObj);

        sortedList.Sort( delegate (DialogTrigger dialogTriggerFirst, DialogTrigger dialogTriggerSecond)
        {
            return dialogTriggerFirst.name.CompareTo(dialogTriggerSecond.name);
        });
    }

    private void OnDestroy()
    {
        triggersList.Clear();
        staticTriggersList.Clear();
        sortedList.Clear();
    }
}
