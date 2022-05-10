using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AudioButtonUI : MonoBehaviour
{

    public Button btn;
    public string name = "Play_UI_Play";

    public void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(EventSystem.current.currentSelectedGameObject.GetComponent<Button>() == btn)
            {
                AkSoundEngine.PostEvent(name, gameObject);
            }
        }
    }
 }
