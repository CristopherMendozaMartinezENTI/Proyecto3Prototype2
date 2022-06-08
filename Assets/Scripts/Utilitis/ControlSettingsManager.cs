using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlSettingsManager : MonoBehaviour
{
    [SerializeField] private Slider sensibilitySlider;
    private GameObject cameraControls;

    void Start()
    {
        cameraControls = GameObject.Find("CameraControls");
        LoadSettings();
    }
    
    public void SetSensibilityValue()
    {
        cameraControls.GetComponent<CameraMouseInput>().SetMouseInputMultiplier(sensibilitySlider.value);
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("SensibilityPreference", sensibilitySlider.value);
        PlayerPrefs.Save();
    }

    private  void LoadSettings()
    {
        if (PlayerPrefs.HasKey("SensibilityPreference"))
            sensibilitySlider.value = PlayerPrefs.GetFloat("SensibilityPreference");
        else
            sensibilitySlider.value = PlayerPrefs.GetFloat("SensibilityPreference");
    }
}
