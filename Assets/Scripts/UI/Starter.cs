using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class Starter : MonoBehaviour
{
    [SerializeField] private SavesPlatformArray _savesPlatformArrayForStart;

    private string _dataPath;

    private void Awake()
    {
        _dataPath = Application.persistentDataPath;
    }

    void Start()
    {
        if (File.Exists(_dataPath + "/WorldThings.txt"))
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            string outputJson;
            outputJson = JsonUtility.ToJson(_savesPlatformArrayForStart, true);
            File.WriteAllText(_dataPath + "/WorldThings.txt", outputJson);
            SceneManager.LoadScene(2);
        }
    }
}
