using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;


public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    public string path;

    public int currentSavedProgression;
    public int test;

    public DATA data;

    public bool saved;



    void Awake()
    {
        Instance = this;

        SetPath();

        Load();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Save();
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            Load();
        }
    }

    void SetPath()
    {
        path = Path.Combine(Application.persistentDataPath, "data.json");
    }

    public void Save()
    {
        if (!saved)
        {
            Load();
            saved = true;

            SetData();
            SerializeData();
        }

        //SetData();
        //SerializeData();
    }

    void SerializeData()
    {
        string dataString = JsonUtility.ToJson(data, true);

        File.WriteAllText(path, dataString);
    }

    /// <summary>
    /// Assigne les valeurs du GameManager à celles du DATA
    /// </summary>
    void SetData()
    {
        data.lastLevel = GameManager.Instance.currentLevel;

        if (data.progressionIndex == data.lastLevel)
        {
            data.progressionIndex = data.lastLevel + 1;

            Debug.Log("data updated");
        }
    }

    public void Load()
    {
        if (File.Exists(path))
        {
            DeserializeData();
        }
        else
        {
            data = new DATA();
        }

        ExploitData();
    }

    void DeserializeData()
    {
        string loadString = File.ReadAllText(path);
        data = JsonUtility.FromJson<DATA>(loadString);
    }

    void ExploitData()
    {
        currentSavedProgression = data.progressionIndex;
    }
}
