using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using PathCreation;
using TMPro;

public class PredatorPathHandler : MonoBehaviour
{
    [SerializeField] private bool _isItBeginersLevel;

    [SerializeField] private float _levelSpeed;
    [SerializeField] private TextMeshProUGUI _currentValueOfMeat;
    [SerializeField] private TextMeshProUGUI _currentOfPrey;

    private int _meatValue;
    private int _totalNewMeat;
    private int _desiredPreyCount;
    private int _currentPreyCount;

    [SerializeField] private GameObject[] _predators;
    [SerializeField] private List<PathesWithOffsets> _pathesForPredators = new List<PathesWithOffsets>();
    [SerializeField] private List<PathesWithOffsets> _pathesForPrey = new List<PathesWithOffsets>();

    [SerializeField] private List<AnimalMovement> _animalsOnTheHunt = new List<AnimalMovement>();
    [SerializeField] private List<AnimalMovement> _animalsPrey = new List<AnimalMovement>();

    [SerializeField] private CameraController _cameraController;

    [SerializeField] private Transform _nearestTransform;

    [SerializeField] private GameObject _finishPopUp;
    [SerializeField] private GameObject _plusMeatPopUp;
    [SerializeField] private UIHalper _uiHalper;

    [SerializeField] private PathesWithOffsets _starterPath;

    private AnimalMovement _firstPredator;
    private string _dataPath;
    private bool _firstEnemyWasSpawned;
    private void Awake()
    {
        _desiredPreyCount = _animalsPrey.Count;
        _currentOfPrey.text = "0/" + _desiredPreyCount.ToString();

        _dataPath = Application.persistentDataPath;
        GameEvents.OnFindNewPredator += MoveCameraToNewCreature;
        GameEvents.OnPreyDied += RemovePreyAndGetMeat;
        GameEvents.OnUpdateMeatStatus += UpdateMeatStatus;
        _meatValue = PlayerPrefs.GetInt("MeatValue");
        _currentValueOfMeat.text = _meatValue.ToString();
        
    }
    private void OnDestroy()
    {
        GameEvents.OnFindNewPredator -= MoveCameraToNewCreature;
        GameEvents.OnPreyDied -= RemovePreyAndGetMeat;
        GameEvents.OnUpdateMeatStatus -= UpdateMeatStatus;
    }


    private void RemovePreyAndGetMeat(GameObject prey, int reward)
    {
        _currentPreyCount += 1;
        _currentOfPrey.text = _currentPreyCount.ToString() + "/" + _desiredPreyCount.ToString();
        _animalsPrey.Remove(prey.GetComponent<AnimalMovement>());

        /*
        _meatValue += reward;
        _totalNewMeat += reward;
        _currentValueOfMeat.text = _meatValue.ToString();
        PlayerPrefs.SetInt("MeatValue", _meatValue);
        _plusMeatPopUp.SetActive(true);
        _plusMeatPopUp.GetComponent<UIHalper>().SetText("+" + reward.ToString());
        */

        
    }

    private void UpdateMeatStatus(int reward)
    {
        _meatValue += reward;
        _totalNewMeat += reward;
        _currentValueOfMeat.text = _meatValue.ToString();
        PlayerPrefs.SetInt("MeatValue", _meatValue);

        if (_currentPreyCount == _desiredPreyCount)
        {
            _finishPopUp.SetActive(true);
            _uiHalper.SetFinishMenuStatus(_desiredPreyCount, _currentPreyCount, _totalNewMeat, true);
        }
        else
        {
            _uiHalper.SetFinishMenuStatus(_desiredPreyCount, _currentPreyCount, _totalNewMeat, false);
        }
    }
    void Start()
    {
        _uiHalper.StartSavesAndEvents();
        SpawnPredatorsFromJson();
        //var nextPredator = _animalsOnTheHunt[Random.Range(0, _animalsOnTheHunt.Count - 1)];
        _cameraController.StartMoveToThePredator(_firstPredator);
        _animalsOnTheHunt.Remove(_firstPredator);
    }

    private void MoveCameraToNewCreature(Transform prey)
    {
        if (_animalsOnTheHunt.Count > 0)
        {
            var nextPredator = _animalsOnTheHunt[Random.Range(0, _animalsOnTheHunt.Count - 1)];
            _cameraController.StartMoveToThePredator(nextPredator, prey);
            _animalsOnTheHunt.Remove(nextPredator);
        }
        else
        {
            _uiHalper.SetFinishMenuStatus(_desiredPreyCount, _currentPreyCount, _totalNewMeat, false);
            _finishPopUp.SetActive(true);
            _cameraController.Fix(prey);
        }
    }
    private void SpawnPredatorsFromJson()
    {
        if (File.Exists(_dataPath + "/WorldThings.txt"))
        {
            string outputJson = File.ReadAllText(_dataPath + "/WorldThings.txt");
            SavesPlatformArray arrayOfPlatforms = JsonUtility.FromJson<SavesPlatformArray>(outputJson);
            SpawnPrey();

            foreach (SavesPlatforms savesPlatforms in arrayOfPlatforms._savesPlatforms)
            {
                if (!savesPlatforms._isPlatformFree && !_firstEnemyWasSpawned && savesPlatforms._isPlatformForHunt)
                {
                    var newPredator = Instantiate(_predators[savesPlatforms._levelOfCreature]).GetComponent<AnimalMovement>();
                    if (_isItBeginersLevel)
                    {
                        newPredator.SetAimForBeginers();
                    }
                    _firstPredator = newPredator;
                    _firstPredator._isItForstCreature = true;
                    _firstPredator._levelSpeed = _levelSpeed;


                    newPredator.SetPathForAnimal(_starterPath.pathCreator, 0, _starterPath.cameraPos);
                    _animalsOnTheHunt.Add(newPredator);
                    _firstEnemyWasSpawned = true;

                }
                else if(!savesPlatforms._isPlatformFree && _firstEnemyWasSpawned && savesPlatforms._isPlatformForHunt)
                {

                    var newPredator = Instantiate(_predators[savesPlatforms._levelOfCreature]).GetComponent<AnimalMovement>();
                    if(_isItBeginersLevel)
                    {
                        newPredator.SetAimForBeginers();
                    }
                    int randomPathID = Random.Range(0, _pathesForPredators.Count - 1);
                    newPredator.SetPathForAnimal(_pathesForPredators[randomPathID].pathCreator, _pathesForPredators[randomPathID].pathOffset, _pathesForPredators[randomPathID].cameraPos);
                    _pathesForPredators.RemoveAt(randomPathID);
                    _animalsOnTheHunt.Add(newPredator);
                }
            }
        }
    }
    private void SpawnPrey()
    {
        foreach (AnimalMovement animalMovement in _animalsPrey)
        {
            if (_isItBeginersLevel)
            {
                int randomPathID = Random.Range(0, _pathesForPrey.Count - 1);
                animalMovement.SetPathForAnimal(_pathesForPrey[randomPathID].pathCreator, _pathesForPrey[randomPathID].pathOffset, _pathesForPrey[randomPathID].cameraPos, true);
                _pathesForPrey.RemoveAt(randomPathID);
            }
            else
            {
                int randomPathID = Random.Range(0, _pathesForPrey.Count - 1);
                animalMovement.SetPathForAnimal(_pathesForPrey[randomPathID].pathCreator, _pathesForPrey[randomPathID].pathOffset, _pathesForPrey[randomPathID].cameraPos);
                _pathesForPrey.RemoveAt(randomPathID);
            }
        }
    }

    public Transform ReturnNearestPreyLookPoint(Transform predatorPosition)
    {
        if (_animalsPrey.Count > 0)
        {
            float distanceToPrey = Mathf.Infinity;
            AnimalMovement _nearestPrey = _animalsPrey[Random.Range(0, _animalsPrey.Count - 1)];


            for (int i = 0; i < _animalsPrey.Count - 1; i++)
            {

                if((_animalsPrey[i].transform.position - predatorPosition.position).magnitude < distanceToPrey)
                {
                    distanceToPrey = (_animalsPrey[i].transform.position - predatorPosition.position).magnitude;
                    _nearestPrey = _animalsPrey[i];
                }
            }

            _nearestTransform = _nearestPrey.ReturnLookPoint();

            return _nearestTransform;
        }
        else
        {
            return transform;
        }

    }
}


[System.Serializable]
public class PathesWithOffsets
{
    public PathCreator pathCreator;
    public float pathOffset;
    public bool cameraPos;
}
