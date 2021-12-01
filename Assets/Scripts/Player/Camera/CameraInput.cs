using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Clase base para las clases que contralaran los inputs de la camara 
public abstract class CameraInput : MonoBehaviour
{
    public abstract float GetHorizontalCameraInput();
    public abstract float GetVerticalCameraInput();
}

