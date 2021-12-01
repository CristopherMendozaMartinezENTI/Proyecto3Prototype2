using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Clase base para las clases que contralaran los inputs del player
public abstract class PlayerInput : MonoBehaviour
{
    public abstract float GetHorizontalMovementInput();
    public abstract float GetVerticalMovementInput();

    public abstract bool IsJumpKeyPressed();
}

