using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AudioButtonUI : MonoBehaviour
{

    public AudioClip clipSound;
    private AudioSource aSource;
    public Button btn;
    public void Start()
    {
        aSource = GetComponent<AudioSource>();
    }
    public void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(EventSystem.current.currentSelectedGameObject.GetComponent<Button>() == btn)
            {
                PlaySound(clipSound);
            }
        }
    }
    

    void PlaySound(AudioClip aClip)
    {
       aSource.PlayOneShot(aClip);
    }
}
