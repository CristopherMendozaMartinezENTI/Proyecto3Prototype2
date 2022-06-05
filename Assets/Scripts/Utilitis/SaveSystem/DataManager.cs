using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{

    private GameData gamedata; 
    public static DataManager instance { get; private set; }

    public void Awake()
    {
        if(instance !=null)
        { 
            Debug.LogError("Found more thean on Data Manager in the scene");
        }
        instance = this;
    }

    public void NewGame()
    {
        this.gamedata = new GameData();
    }

    public void LoadGame()
    {
        //Recoger la informacion del archivo de guardado y cargar-lo.

        //si no hay data possible para cargar, iniciar un juego nuevo.
        if (this.gamedata == null)
        {
            Debug.Log("No data found. Creating a new game!");
            NewGame();
        }

        //Cargar la informacion del archivo de guardado al resto de scripts
    }

    public void SaveGame()
    {
        // enviar la informacion a los otros archivos para que puedan actualizasrla

        //guardar la informacion en un archivo de guardado.
    }

}
