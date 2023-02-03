using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class UIHalper : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _finishPrey;
    [SerializeField] private TextMeshProUGUI _finishReward;

    [SerializeField] private TextMeshProUGUI _text;
    public int _levelID;
    [SerializeField] private bool _isItLastLevel;

    [SerializeField] private bool _isItMainUIHapler;
    private int _newMeat;
    private int _desiredPreyCount;
    private int _playersPreyCount;

    private void Start()
    {

    }
    public void StartSavesAndEvents()
    {
        
        PlayerPrefs.SetInt("Level", _levelID);
        TinySauce.OnGameStarted((_levelID - 1).ToString());
        Debug.Log("start: " + (_levelID - 1).ToString());
    }
    public void SetStatusOfObject()
    {
        gameObject.SetActive(false);
    }
    public void SetText(string text)
    {
        _text.text = text;
    }
    public void SetFinishMenuStatus(int desiredPreyCount, int playersPreyCount, int newMeat, bool openNextLevel)
    {
        _finishPrey.text = playersPreyCount.ToString() + "/" + desiredPreyCount.ToString();
        _finishReward.text = "+" + newMeat.ToString();

        if (openNextLevel && !_isItLastLevel)
        {
            _levelID += 1;
            PlayerPrefs.SetInt("Level", _levelID);
        }

        _newMeat = newMeat;
        _desiredPreyCount = desiredPreyCount;
        _playersPreyCount = playersPreyCount;
    }



    public void LoadLevel()
    {
        FinishLevel();
        SceneManager.LoadScene(_levelID);
    }
    public void LoadMeinMenu()
    {
        Debug.Log("finish: " + (_levelID - 1).ToString());
        TinySauce.OnGameFinished(false, _newMeat, (_levelID - 1).ToString());
        SceneManager.LoadScene("MainScene");
        DOTween.KillAll();
    }

    private void FinishLevel()
    {
        if (_desiredPreyCount == _playersPreyCount)
        {
            TinySauce.OnGameFinished(true, _newMeat, (_levelID - 2).ToString());
            Debug.Log("finish: " + (_levelID - 2).ToString() + " true");
        }
        else
        {
            TinySauce.OnGameFinished(false, _newMeat, (_levelID - 1).ToString());
            Debug.Log("finish: " + (_levelID - 1).ToString() + " false");
        }

        DOTween.KillAll();
    }
}
