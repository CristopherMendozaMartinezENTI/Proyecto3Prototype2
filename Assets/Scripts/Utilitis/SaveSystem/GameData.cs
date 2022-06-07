using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int currentLevel;
    public Vector3 lastCheckpoint;
    public Quaternion lastRotation;

    //Valores defoult
    public GameData()
    {
        this.currentLevel = 0;
        lastCheckpoint = Vector3.zero;
        lastRotation = Quaternion.identity;
    }
}