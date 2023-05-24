using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class PersistenceManager : MonoBehaviour
{
    public static PersistenceManager Instance;

    public GameData data;

    [SerializeField] string SaveName = "Save1";

    public List<IDataPersistence> dataPersistenceList;

    public bool firstLoad = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        dataPersistenceList = FindAllDataPersistenceObjects();

        LoadAllData();
    }

    private void Update() ///////// temp
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            SaveAllData();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            print(File.ReadAllText(Application.dataPath + "/" + SaveName + ".txt"));
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            string clearDataString = JsonConvert.SerializeObject(new GameData());

            File.WriteAllText(Application.dataPath + "/" + SaveName + ".txt", clearDataString);

            LoadAllData();
        }
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> list = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

        return new List<IDataPersistence>(list);
    }

    void SaveAllData()
    {
        foreach (IDataPersistence dataPersistence in dataPersistenceList)
        {
            dataPersistence.Save();
        }

        //string dataString = JsonUtility.ToJson(data); //encrypt this string
        string dataString = JsonConvert.SerializeObject(data, Formatting.Indented);

        File.WriteAllText(Application.dataPath + "/" + SaveName + ".txt", dataString); 

        //PlayerPrefs.SetString(SaveName, dataString);
    }
    void LoadAllData()
    {
        //string dataString = PlayerPrefs.GetString(SaveName); // decrypt this string
        string dataString = File.ReadAllText(Application.dataPath + "/" + SaveName + ".txt");

        //data = JsonUtility.FromJson<GameData>(dataString);
        data = JsonConvert.DeserializeObject<GameData>(dataString);

        foreach (IDataPersistence dataPersistence in dataPersistenceList)
        {
            dataPersistence.Load();
        }
    }

    private void OnApplicationQuit()
    {
        print("Quit ---- firdtLoad = " + firstLoad);
        if (firstLoad == true)
            SaveAllData();
    }
    private void OnApplicationPause(bool pause)
    {
        print("Pause ---- firdtLoad = " + firstLoad);
        if (firstLoad == true)
            SaveAllData();
    }
}

[Serializable]
public class GameData
{
    //TimeEvents
    public SerilizableDictionary<uint, DateTime> Events;

    //Resources
    public int Gold;
    public int Cash;

    //Grid -> unlock - used - structures 
    public GridData[,] GridDaddy;
}
