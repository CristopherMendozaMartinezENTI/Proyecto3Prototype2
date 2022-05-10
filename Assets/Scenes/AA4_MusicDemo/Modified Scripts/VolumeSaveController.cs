using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSaveController : MonoBehaviour
{
    [SerializeField] private Slider thisSlider;

    public float masterVolume;
    public float musicVolume;
    public float soundVolume;

    public void SetVolume(string currentValue)
    {
        float sliderValue = thisSlider.value;

        if (currentValue == "Master")
        {
            masterVolume = thisSlider.value;
            AkSoundEngine.SetRTPCValue("Master", masterVolume);
        }

        if (currentValue == "Music")
        {
            musicVolume = thisSlider.value;
            AkSoundEngine.SetRTPCValue("Music", musicVolume);
        }

        if (currentValue == "Sound")
        {
            soundVolume = thisSlider.value;
            AkSoundEngine.SetRTPCValue("Sounds", soundVolume);
        }
    }
}
