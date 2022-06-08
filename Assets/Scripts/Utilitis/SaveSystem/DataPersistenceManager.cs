using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;
    public bool newGame;

    public GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;

    public static DataPersistenceManager instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else Destroy(gameObject);
    }

    private void Start()
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
    }

    public void NewGame()
    {
        this.gameData = new GameData();
        Debug.Log("Creating new data");

    }

    public void LoadGame()
    {
        // Cargar cualquier info a gametada con el datahandler
        this.gameData = dataHandler.Load();

        // Si no hay data inicializa una nueva y empieza un juego nuevo.
        if (this.gameData == null)
        {
            Debug.Log("No data was found. Initializing data to defaults.");
            newGame = true;
            NewGame();
        }

        //Passar la info de todos los scripts que la utilizan para guardarla
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        //Enviar la info a todos los scripts que la necesitan
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(gameData);
        }
        Debug.Log("lvl: " + gameData.currentLevel + "postn: " + gameData.lastCheckpoint);
        //Guardar la data utilizando el datahandler
        dataHandler.Save(gameData);
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
            .OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}