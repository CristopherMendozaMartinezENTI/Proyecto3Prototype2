using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
    [System.Serializable]
    public class AudioTriggers
    {
        public GameObject Trigger;
        public string Dialog;
    }

    [SerializeField]
    public List<AudioTriggers> triggersList = new List<AudioTriggers>();

    private List<GameObject> listGO = new List<GameObject>(); 

    private void Start()
    {
        foreach (GameObject gameObj in GameObject.FindObjectsOfType<GameObject>())
        {
            if (gameObj.name.Contains("audio"))
            {
                listGO.Add(gameObj);
            }
        }

        listGO.Sort( delegate (GameObject i1, GameObject i2)
        {
            return i1.name.CompareTo(i2.name);
        });


        for (int i = 0; i < triggersList.Count; i++)
        {
            listGO[i].GetComponent<SoundTrigger>().DialogText = triggersList[i].Dialog;
        }
    }


}
