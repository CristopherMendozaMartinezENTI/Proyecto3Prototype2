using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
    [System.Serializable]
    public class AudioTriggers
    {
        public GameObject Trigger;
        public AudioDialogo[] Conversation;
    }

    [System.Serializable]
    public class AudioDialogo 
    {
        public string Dialog;
        public AudioClip Audio;
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


        //Relaciono el texto de la lista de los triggers en la escena con el texto de la lista creada en el editor:
        for (int i = 0; i < triggersList.Count; i++)
        {
            listGO[i].GetComponent<SoundTrigger>().DialogText = triggersList[i].Conversation[0].Dialog;
            if (triggersList[i].Conversation.Length <= 1) 
            { 
                //Función de más de una conversación...
            }
        }
    }


}
