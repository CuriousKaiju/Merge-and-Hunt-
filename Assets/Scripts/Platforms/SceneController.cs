using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class SceneController : MonoBehaviour
{
    public GameObject[] _mergeCreaturesArray;
    [SerializeField] private PlatformsHandler _platformsHandler;
    [SerializeField] private GameObject _animalPlatform;
    [SerializeField] private PlatformsSaves _platformSaves;
    [SerializeField] private int _nextLevel;
    [SerializeField] private TextMeshProUGUI _levelText;

    [SerializeField] private GameObject _dragTextObject;
    [SerializeField] private Animator _dragAnimator;
    [SerializeField] private GameObject _blackScreenAppears;
    [SerializeField] private LevelMap _levelMap;


    [Header("CURRENCY")]
    [SerializeField] private int[] _arrayOfCost;
    public int _currentMeatCount;
    [SerializeField] private TextMeshProUGUI _currntMeatValue;
    [SerializeField] private TextMeshProUGUI _buyButtonValue;
    [SerializeField] private Button _buyNewAnimalButton;
    [SerializeField] private Button _huntButton;
    [SerializeField] private int _startMeatValue;
    [SerializeField] private Transform _currentValueTransform;
    [SerializeField] private Vector3 _punchScale;
    [SerializeField] private float _punchScaleDuration;
    [SerializeField] private MergeLocationHandler _mergeLocationHandler;
    private int _currentBuyLevel;
    private int _visualLevel;
    private void Awake()
    {
        SpawnCostArray();
        SetMeatStatus();

        if (PlayerPrefs.HasKey("Level"))
        {
            _nextLevel = PlayerPrefs.GetInt("Level");
            _levelText.text = "Level " + (_nextLevel - 1);
        }
        else
        {
            _nextLevel = 2;
            _levelText.text = "Level " + (_nextLevel - 1);
        }

        _levelMap.SetLevelMapStatus(_nextLevel - 1);
        _mergeLocationHandler.SetLocation(_nextLevel - 1);

        GameEvents.OnMoneyChange += UnlockNewSlotAndChangeMeat;
    }

    private void SpawnCostArray()
    {
        _arrayOfCost = new int[300];
        int nextPrice = 10;

        for (int i = 0; i < 300; i++)
        {
            if (i == 0)
            {
                _arrayOfCost[i] = nextPrice;
            }
            else if (nextPrice < 50)
            {
                nextPrice += 5;
                _arrayOfCost[i] = nextPrice;
            }
            else
            {
                nextPrice += 10;
                _arrayOfCost[i] = nextPrice;
            }
        }
    }
    private void OnDestroy()
    {
        GameEvents.OnMoneyChange -= UnlockNewSlotAndChangeMeat;
    }
    public GameObject GetNewCreature(int creatureLevel)
    {
        return _mergeCreaturesArray[creatureLevel];
    }

    public GameObject GetSpawnCreature()
    {
        return _mergeCreaturesArray[0];
    }

    public void BuyNewAnimal(int animalLevel)
    {
        int idOfnewAnimal = animalLevel;



        if (_currentBuyLevel > 27)
        {
            idOfnewAnimal += 1;
        }

        if (_currentBuyLevel > 100)
        {
            idOfnewAnimal += 1;
        }

        if (_currentBuyLevel > 151)
        {
            idOfnewAnimal += 1;
        }

        if (_currentBuyLevel > 200)
        {
            idOfnewAnimal += 1;
        }

        if (_currentBuyLevel > 251)
        {
            idOfnewAnimal += 1;
        }

        _currentValueTransform.DOKill();
        _currentValueTransform.localScale = new Vector3(1, 1, 1);
        _currentValueTransform.DOPunchScale(_punchScale, _punchScaleDuration);

        ChangeMeat(-_arrayOfCost[_currentBuyLevel]);

        var freePlatform = _platformsHandler.GetFreePlatform();

        if (freePlatform != null)
        {
            var newAnimalPlatform = Instantiate(_animalPlatform, freePlatform.transform).GetComponent<PlatformsAnimals>();
            var newPlatformsCreature = Instantiate(_mergeCreaturesArray[idOfnewAnimal], newAnimalPlatform.transform).GetComponent<MergeCreature>();
            newAnimalPlatform.SetMergeCreatutureAfterSpawn(newPlatformsCreature, idOfnewAnimal);
            freePlatform.UpdatePlatformStatus(newAnimalPlatform.transform);
            _platformsHandler.RewriteFreePlatformsList();
        }

        _platformsHandler.CheckHuntingGroup();
    }
    public void SpawnNewAnimalFromSaves(Platform platformForAnimal, int animalLevel)
    {
        var freePlatform = platformForAnimal;
        var newAnimalPlatform = Instantiate(_animalPlatform, freePlatform.transform).GetComponent<PlatformsAnimals>();
        var newPlatformsCreature = Instantiate(_mergeCreaturesArray[animalLevel], newAnimalPlatform.transform).GetComponent<MergeCreature>();
        newAnimalPlatform.SetMergeCreatutureAfterSpawn(newPlatformsCreature, animalLevel);
        freePlatform.UpdatePlatformStatus(newAnimalPlatform.transform);
        _platformsHandler.RewriteFreePlatformsList();
    }
    public GameObject GetMergeCreature(int creatureID)
    {
        return _mergeCreaturesArray[creatureID];
    }
    public void StartHunt()
    {
        _platformSaves.SaveToJson();
        _blackScreenAppears.SetActive(true);
    }
    public void LoadLevel()
    {
        SceneManager.LoadScene(_nextLevel);
    }
        
    private void UnlockNewSlotAndChangeMeat(int meatValue)
    {
        _currentMeatCount += meatValue;
        _currntMeatValue.text = _currentMeatCount.ToString();

        if (_currentMeatCount < _arrayOfCost[_currentBuyLevel])
        {
            _buyNewAnimalButton.interactable = false;
        }

        _platformsHandler.RewriteFreePlatformsList();
        PlayerPrefs.SetInt("BuyLevel", _currentBuyLevel);
        PlayerPrefs.SetInt("MeatValue", _currentMeatCount);
    }

    public void SetHuntButtonStatus(bool status)
    {
        _huntButton.interactable = status;
    }

    private void ChangeMeat(int meatValue)
    {
        _currentMeatCount += meatValue;
        _currntMeatValue.text = _currentMeatCount.ToString();
        _currentBuyLevel += 1;
        if(_currentBuyLevel >= _arrayOfCost.Length)
        {
            _currentBuyLevel = _arrayOfCost.Length - 1;
        }

        _buyButtonValue.text = _arrayOfCost[_currentBuyLevel].ToString();

        if (_currentMeatCount < _arrayOfCost[_currentBuyLevel])
        {
            _buyNewAnimalButton.interactable = false;
        }

        PlayerPrefs.SetInt("BuyLevel", _currentBuyLevel);
        PlayerPrefs.SetInt("MeatValue", _currentMeatCount); 
    }

    private void SetMeatStatus()
    {
        if (PlayerPrefs.HasKey("BuyLevel"))
        {
            _currentBuyLevel = PlayerPrefs.GetInt("BuyLevel");
        }
        else
        {
            _currentBuyLevel = 0;
        }


        if (PlayerPrefs.HasKey("MeatValue"))
        {
            _currentMeatCount = PlayerPrefs.GetInt("MeatValue");
        }
        else
        {
            _currentMeatCount = _startMeatValue;
            PlayerPrefs.SetInt("MeatValue", _currentMeatCount);
        }

        _currntMeatValue.text = _currentMeatCount.ToString();
        _buyButtonValue.text = _arrayOfCost[_currentBuyLevel].ToString();

        if (_currentMeatCount < _arrayOfCost[_currentBuyLevel])
        {
            _buyNewAnimalButton.interactable = false;
        }
        else
        {
            _buyNewAnimalButton.interactable = _platformsHandler.ReturnFreePlatformsCount();
        }
    }

    public void UpdateButtonState()
    {
        if (_currentMeatCount < _arrayOfCost[_currentBuyLevel])
        {
            _buyNewAnimalButton.interactable = false;
        }
        else
        {
            _buyNewAnimalButton.interactable = _platformsHandler.ReturnFreePlatformsCount();
        }
    }

    public void ActivateDragTextAnimation()
    {
        _dragTextObject.SetActive(true);
    }
    public void CloseDragTextAnimation()
    {
        _dragAnimator.SetTrigger("Finish");
    }
    
    public void RemoveMeatSaves()
    {
        PlayerPrefs.DeleteAll();
    }
}
