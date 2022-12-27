using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlatformsSaves : MonoBehaviour
{
    [SerializeField] private PlatformsHandler _platformHandler;
    [SerializeField] private SavesPlatformArray _savesPlatformArrayForStart;
    
    private string _dataPath;

    private bool _firstLoad;

    private void Awake()
    {
        _dataPath = Application.persistentDataPath;
        Debug.Log(_dataPath);
    }
    private void Start()
    {
        if (File.Exists(_dataPath + "/WorldThings.txt"))
        {
            string outputJson = File.ReadAllText(_dataPath + "/WorldThings.txt");
            SavesPlatformArray arrayOfPlatforms = JsonUtility.FromJson<SavesPlatformArray>(outputJson);
            _platformHandler.SpawnAnimalsFromSaves(arrayOfPlatforms);
        }
        else
        {
            _platformHandler.SetTutorialStatus(true);
            _platformHandler.SpawnAnimalsFromSaves(_savesPlatformArrayForStart);
        }

        _platformHandler.CheckHuntingGroup();
    }
    void OnApplicationPause(bool pauseStatus)
    {
        if (_firstLoad)
        {
            SaveToJson();
        }
        _firstLoad = true;
    }
    private void OnApplicationQuit()
    {
        if (_firstLoad)
        {
            SaveToJson();
        }
    }

    public void SaveToJson()
    {
        string outputJson;
        outputJson = JsonUtility.ToJson(_platformHandler.ReturnSavesPlatformArray(), true);
        File.WriteAllText(_dataPath + "/WorldThings.txt", outputJson);
    }
}
