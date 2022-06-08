using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Este script permite controlar la camara mediante el uso del raton
public class CameraMouseInput : CameraInput
{
    [SerializeField] private string mouseHorizontalAxis = "Mouse X";
    [SerializeField] private string mouseVerticalAxis = "Mouse Y";
    [SerializeField] private bool invertHorizontalInput = false;
    [SerializeField] private bool invertVerticalInput = false;
    [SerializeField] private float mouseInputMultiplier = 0.01f;

	public override float GetHorizontalCameraInput()
    {
        float _input = Input.GetAxisRaw(mouseHorizontalAxis);
            
        if(Time.timeScale > 0f && Time.deltaTime > 0f)
        {
            _input /= Time.deltaTime;
            _input *= Time.timeScale;
        }
        else
            _input = 0f;

        _input *= mouseInputMultiplier;
        if(invertHorizontalInput)
            _input *= -1f;

        return _input;
    }

    public override float GetVerticalCameraInput()
    {
        float _input = -Input.GetAxisRaw(mouseVerticalAxis);
        if(Time.timeScale > 0f && Time.deltaTime > 0f)
        {
            _input /= Time.deltaTime;
            _input *= Time.timeScale;
        }
        else
            _input = 0f;

        _input *= mouseInputMultiplier;

        if(invertVerticalInput)
            _input *= -1f;

        return _input;
    }

    public void SetMouseInputMultiplier(float multiplier)
    {
        mouseInputMultiplier = multiplier;
    }
}

