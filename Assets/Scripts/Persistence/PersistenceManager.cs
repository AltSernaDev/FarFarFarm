using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistenceManager : MonoBehaviour
{
    public static PersistenceManager Instance;

    public GameData data;

    [SerializeField] string SaveName = "Save1";

    public List<IDataPersistence> dataPersistenceList; // find and add new 

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
            print(PlayerPrefs.GetString(SaveName));
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            string dataString = JsonUtility.ToJson(new GameData()); 

            PlayerPrefs.SetString(SaveName, dataString);

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

        string dataString = JsonUtility.ToJson(data); //encrypt this string

        PlayerPrefs.SetString(SaveName, dataString);
    }
    void LoadAllData()
    {
        string dataString = PlayerPrefs.GetString(SaveName); // decrypt this string

        data = JsonUtility.FromJson<GameData>(dataString);

        foreach (IDataPersistence dataPersistence in dataPersistenceList)
        {
            dataPersistence.Load();
        }
    }

    private void OnApplicationQuit()
    {
        SaveAllData();
    }
    private void OnApplicationPause(bool pause)
    {
        SaveAllData();
    }
}

[Serializable]
public class GameData
{
    //TimeEvents
    public Dictionary<uint, DateTime> Events;

    //Resources
    public int Gold;
    public int Cash;

    //Grid -> unlock - used - structures 
    public Grid[,] grids; //check
}
