using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformsHandler : MonoBehaviour
{
    [SerializeField] private Platform[] _platforms;
    [SerializeField] private List<Platform> _freePlatforms = new List<Platform>();
    [SerializeField] private List<Platform> _huntOpenedStashPlatforms = new List<Platform>();
    [SerializeField] private SavesPlatformArray _arrayForSaves;
    [SerializeField] private GameObject _animalPlatform;
    [SerializeField] private SceneController _sceneController;
    [SerializeField] private GameObject _tutorial;

    private void Awake()
    {
        GameEvents.OnIncreasePlatformsPrice += UpdatePlatformPrice;
    }
    private void OnDestroy()
    {
        GameEvents.OnIncreasePlatformsPrice -= UpdatePlatformPrice;
    }

    void Start()
    {
        RewriteFreePlatformsList();
    }

    void Update()
    {

    }

    public void SetTutorialStatus(bool status)
    {
        _tutorial.SetActive(status);
    }


    public void RewriteFreePlatformsList()
    {
        _freePlatforms = new List<Platform>();
        _huntOpenedStashPlatforms = new List<Platform>();

        foreach (Platform platform in _platforms)
        {
            if (platform._isPlatformFree && platform._isItBoughtPlatform)
            {
                _freePlatforms.Add(platform);
            }
            else if (!platform._isItBoughtPlatform)
            {
                if (_sceneController._currentMeatCount >= platform._openPrice)
                {
                    platform.SetBuyStatus(true);
                }
                else
                {
                    platform.SetBuyStatus(false);
                }
            }

            if (platform._isItHuntGroupPlatform && platform._isItBoughtPlatform && !platform._isPlatformFree)
            {
                _huntOpenedStashPlatforms.Add(platform);
            }
        }


        _sceneController.UpdateButtonState();

    }
    private void UpdatePlatformPrice(int pastPrice)
    {
        foreach (Platform platform in _platforms)
        {
            if (!platform._isItBoughtPlatform)
            {
                platform._openPrice = pastPrice * 3;
                platform.SetDoesntBuyStatus();
            }
        }
    }

    public void CheckHuntingGroup()
    {
        if (_huntOpenedStashPlatforms.Count == 0)
        {
            _sceneController.ActivateDragTextAnimation();
            _sceneController.SetHuntButtonStatus(false);
        }
        else
        {
            _sceneController.CloseDragTextAnimation();
            _sceneController.SetHuntButtonStatus(true);
        }
    }
    public void HighlightMergePlatforms(int animalLevel)
    {
        foreach (Platform platform in _platforms)
        {
            if (!platform._isPlatformFree)
            {
                if (platform.GetCurrentCreatureLevel() == animalLevel)
                {
                    platform.SetHighlightStatus();
                }
            }
        }
    }
    public void ToBaseMergePlatforms()
    {
        foreach (Platform platform in _platforms)
        {
            platform.SetBaseStatus();
        }
    }

    public Platform GetFreePlatform()
    {
        if (_freePlatforms.Count > 0)
        {
            return _freePlatforms[Random.Range(0, _freePlatforms.Count - 1)];
        }
        else
        {
            return null;
        }
    }

    public SavesPlatformArray ReturnSavesPlatformArray()
    {
        SavesPlatformArray newSavesPlatformArray = new SavesPlatformArray(_platforms.Length);

        for (int i = 0; i < _platforms.Length; i++)
        {
            newSavesPlatformArray._savesPlatforms[i] = new SavesPlatforms();
            newSavesPlatformArray._savesPlatforms[i]._isPlatformFree = _platforms[i]._isPlatformFree;

            newSavesPlatformArray._savesPlatforms[i]._isPlatformBought = _platforms[i]._isItBoughtPlatform;
            newSavesPlatformArray._savesPlatforms[i]._isPlatformForHunt = _platforms[i]._isItHuntGroupPlatform;
            newSavesPlatformArray._savesPlatforms[i]._platformCost = _platforms[i]._openPrice;

            if (!newSavesPlatformArray._savesPlatforms[i]._isPlatformFree)
            {
                newSavesPlatformArray._savesPlatforms[i]._levelOfCreature = _platforms[i].GetCurrentCreatureLevel();
            }
        }

        return newSavesPlatformArray;
    }

    public void SpawnAnimalsFromSaves(SavesPlatformArray savePlatformArray)
    {
        _arrayForSaves = savePlatformArray;

        for (int i = 0; i < _platforms.Length; i++)
        {
            _platforms[i]._isItBoughtPlatform = savePlatformArray._savesPlatforms[i]._isPlatformBought;
            _platforms[i]._isItHuntGroupPlatform = savePlatformArray._savesPlatforms[i]._isPlatformForHunt;
            _platforms[i]._openPrice = savePlatformArray._savesPlatforms[i]._platformCost;

            if (!savePlatformArray._savesPlatforms[i]._isPlatformFree)
            {
                _sceneController.SpawnNewAnimalFromSaves(_platforms[i], savePlatformArray._savesPlatforms[i]._levelOfCreature);
            }
            else if (!savePlatformArray._savesPlatforms[i]._isPlatformBought)
            {
                _platforms[i].SetDoesntBuyStatus();
            }
        }
        RewriteFreePlatformsList();
    }
    public bool ReturnFreePlatformsCount()
    {
        if (_freePlatforms.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}

[System.Serializable]
public class SavesPlatforms
{
    public bool _isPlatformForHunt;
    public bool _isPlatformBought;
    public bool _isPlatformFree;
    public int _levelOfCreature;
    public int _platformCost;

    public SavesPlatforms()
    {
        _isPlatformForHunt = false;
        _isPlatformBought = true;
        _isPlatformFree = true;
        _levelOfCreature = 0;
        _platformCost = 100;
    }
}

[System.Serializable]
public class SavesPlatformArray
{
    public SavesPlatforms[] _savesPlatforms;

    public SavesPlatformArray(int arrayCount)
    {
        _savesPlatforms = new SavesPlatforms[arrayCount];
    }
    
}
