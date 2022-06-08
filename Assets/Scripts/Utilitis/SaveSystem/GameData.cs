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
        lastCheckpoint = new Vector3(-33.5f, 0.375f, 12.25005f);
        lastRotation = Quaternion.Euler(new Vector3(0, -270, 0));
    }
}