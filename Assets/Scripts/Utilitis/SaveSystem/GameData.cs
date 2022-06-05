using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class GameData
{
    public int level;

    //Los valores que se definen en el constructor se utilizan cuando se crea un juego nuevo.
    public GameData()
    {
        this.level = 1;
    }
}
