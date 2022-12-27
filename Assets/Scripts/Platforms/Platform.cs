using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lofelt.NiceVibrations;
using TMPro;
using DG.Tweening;
public class Platform : MonoBehaviour
{
    public bool _isPlatformFree;
    public bool _isItBoughtPlatform;
    public bool _isItHuntGroupPlatform;
    public int _openPrice;

    [Header("ANIMAL")]
    [SerializeField] private Transform _currentAnimal;
    [SerializeField] private PlatformsAnimals _platformsAnimals;

    [Header("PLATFORM")]
    [SerializeField] private GameObject _buyCanvas;
    [SerializeField] private GameObject _borders;
    [SerializeField] private MeshRenderer _meshrenderer;
    [SerializeField] private Material _baseMaterial;
    [SerializeField] private Material _highlightMaterial;
    [SerializeField] private Material _redBuyMaterial;
    [SerializeField] private TextMeshProUGUI _costValue;
    [SerializeField] private GameObject _buyParticles;

    [Header("FEED BACK")]
    [SerializeField] private GameObject _particles;
    [SerializeField] private HapticSource _hapticSource;
    [SerializeField] private Color _redPlatformText;


    public Transform GetCurrentAnimal()
    {
        _platformsAnimals.SetHighlightStatusOfCreature();
        return _currentAnimal;
    }
    public void SetAnimalToPlatform(Transform animal)
    {
        animal.SetParent(transform);
        animal.transform.localPosition = new Vector3(0, 0, 0);
        _platformsAnimals = animal.gameObject.GetComponent<PlatformsAnimals>();
        _platformsAnimals.SetBaseStatusOfCreature();
        _currentAnimal = animal;
        _isPlatformFree = false;

        _platformsAnimals.ActivateBounce();
    }
    public void TryToBuyPlatform(int playersMoney)
    {
        if (playersMoney >= _openPrice)
        {
            _isPlatformFree = true;
            _isItBoughtPlatform = true;
            SetBoughtStatus();
            GameEvents.CallOnMoneyChange(-_openPrice);
        }
        else
        {
            transform.DOKill();
            transform.localScale = new Vector3(1, 1, 1);
            transform.DOPunchScale(Vector3.up, 0.5f);
        }
    }
    public void SetDoesntBuyStatus()
    {
        _buyCanvas.SetActive(true);
        _borders.SetActive(true);
        _costValue.text = _openPrice.ToString();
        _meshrenderer.material = _redBuyMaterial;
    }

    public void SetBoughtStatus()
    {
        _buyCanvas.SetActive(false);
        _borders.SetActive(false);
        _buyParticles.SetActive(true);
        SetBaseStatus();
        GameEvents.CallOnIncreasePlatformsPrice(_openPrice);
    }
    public void SetPlatformStatusToNull()
    {
        _currentAnimal = null;
        _platformsAnimals = null;
        _isPlatformFree = true;
    }
    public int GetCurrentCreatureLevel()
    {
        return _platformsAnimals._animalLevel;
    }
    public void SetHighlightStatus()
    {
        _meshrenderer.material = _highlightMaterial;
    }
    public void SetBuyStatus(bool status)
    {
        if (status)
        {
            _costValue.color = Color.white;
        }
        else
        {
            _costValue.color = _redPlatformText;
        }
    }
    public void SetBaseStatus()
    {
        if (_isItBoughtPlatform)
        {
            _meshrenderer.material = _baseMaterial;
        }
        else
        {
            _meshrenderer.material = _redBuyMaterial;
        }
    }
    public void ClearPlatformAndSpawnanimals(GameObject newLevelAnimal, int level)
    {
        _platformsAnimals.RemoveAnimal();
        var newCreature = Instantiate(newLevelAnimal, _platformsAnimals.transform);
        newCreature.GetComponent<MergeCreature>().SetPosition();
        _platformsAnimals.SetMergeCreatuture(newCreature.GetComponent<MergeCreature>(), level);
        _particles.SetActive(true);
        _hapticSource.Play();
        _particles.GetComponent<ParticleSystem>().Play();
        _isPlatformFree = false;
    }

    public void SpawnNewAnimalHere(GameObject newLevelAnimal)
    {
        var newCreature = Instantiate(newLevelAnimal, _platformsAnimals.transform);
        newCreature.GetComponent<MergeCreature>().SetPosition();


        _isPlatformFree = false;
    }

    public void UpdatePlatformStatus(Transform animal)
    {
        _platformsAnimals = animal.gameObject.GetComponent<PlatformsAnimals>();
        _currentAnimal = animal;
        _isPlatformFree = false;
    }
}
