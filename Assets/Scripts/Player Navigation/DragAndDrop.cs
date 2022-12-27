using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DragAndDrop : MonoBehaviour
{
    [Header("LAYERS")]
    [SerializeField] private LayerMask _platformLayer;
    [SerializeField] private LayerMask _touchPointLayer;

    [Header("TOUCH POINT")]
    [SerializeField] private Transform _touchPoint;
    [SerializeField] private Vector3 _touchPointOffset;

    [Header("NAVIGATION")]
    [SerializeField] private Transform _selectedPlatform;
    [SerializeField] private Transform _selectedAnimal;
    private bool _isFingerOnTheScreen;
    private bool _isFingerAboveBuyPlatform;

    [Header("PLATFORMS")]
    [SerializeField] private PlatformsHandler _platformsHandler;
    [SerializeField] private SceneController _sceeControllern;
    [SerializeField] private PlatformsAnimals _animalPlatform;

    [SerializeField] private int _lastMergeLevel;

    void Update()
    {
        TouchHandler();
    }

    private void TouchHandler()
    {
        HitRayAndMoveTouchPoint();

        if (Input.GetMouseButtonDown(0))
        {
            HitRayAndTryToFindPlatform();
            _platformsHandler.SetTutorialStatus(false);
        }
        else if (_isFingerOnTheScreen)
        {
            if (Input.GetMouseButtonUp(0))
            {
                HitRayAndTryToSetFigureOnThePlatform();
                _isFingerOnTheScreen = false;
                _platformsHandler.ToBaseMergePlatforms();
            }
        }
        else if (_isFingerAboveBuyPlatform)
        {
            if (Input.GetMouseButtonUp(0))
            {
                HitRayAndTryToBuyPlatform();
                _isFingerAboveBuyPlatform = false;
            }
        }
    }


    private void HitRayAndTryToFindPlatform()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, _platformLayer))
        {
            _selectedPlatform = hit.collider.transform;
            Platform selectedPlatform = hit.collider.gameObject.GetComponent<Platform>();

            if (!selectedPlatform._isPlatformFree && selectedPlatform._isItBoughtPlatform)
            {
                _selectedAnimal = selectedPlatform.GetCurrentAnimal();
                _selectedAnimal.SetParent(_touchPoint);
                _selectedAnimal.transform.localPosition = new Vector3(0, 0, 0);
                _isFingerOnTheScreen = true;
                selectedPlatform.SetPlatformStatusToNull();
                selectedPlatform._isPlatformFree = true;

                if (_selectedAnimal.GetComponent<PlatformsAnimals>()._animalLevel != _lastMergeLevel)
                {
                    _platformsHandler.HighlightMergePlatforms(_selectedAnimal.GetComponent<PlatformsAnimals>()._animalLevel);
                }
            }
            else if (!selectedPlatform._isItBoughtPlatform)
            {
                _selectedPlatform = hit.collider.transform;
                _isFingerAboveBuyPlatform = true;
            }
        }
    }

    private void HitRayAndMoveTouchPoint()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, _touchPointLayer))
        {
            _touchPoint.position = hit.point + _touchPointOffset;
        }
    }

    private void HitRayAndTryToSetFigureOnThePlatform()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, _platformLayer))
        {
            Platform selectedPlatform = hit.collider.gameObject.GetComponent<Platform>();

            if (selectedPlatform._isPlatformFree && hit.collider.gameObject != _selectedPlatform.gameObject && selectedPlatform._isItBoughtPlatform)
            {
                selectedPlatform.SetAnimalToPlatform(_selectedAnimal);

                _selectedPlatform.GetComponent<Platform>().SetPlatformStatusToNull();
                _selectedPlatform = null;
                _selectedAnimal = null;
            }
            else if (selectedPlatform._isPlatformFree && hit.collider.gameObject == _selectedPlatform.gameObject)
            {
                selectedPlatform.SetAnimalToPlatform(_selectedAnimal);
                _selectedPlatform = null;
                _selectedAnimal = null;
            }
            else if (!selectedPlatform._isPlatformFree)
            {
                if (selectedPlatform.GetCurrentCreatureLevel() == _selectedAnimal.GetComponent<PlatformsAnimals>()._animalLevel && selectedPlatform.GetCurrentCreatureLevel() != _lastMergeLevel)
                {
                    selectedPlatform.GetComponent<Platform>().ClearPlatformAndSpawnanimals(_sceeControllern._mergeCreaturesArray[_selectedAnimal.GetComponent<PlatformsAnimals>()._animalLevel + 1], _selectedAnimal.GetComponent<PlatformsAnimals>()._animalLevel + 1);

                    Destroy(_selectedAnimal.gameObject);
                    _selectedPlatform = null;
                    _selectedAnimal = null;
                }
                else
                {
                    var creatureLevel = selectedPlatform.GetCurrentCreatureLevel();
                    selectedPlatform.GetComponent<Platform>().ClearPlatformAndSpawnanimals(_sceeControllern._mergeCreaturesArray[_selectedAnimal.GetComponent<PlatformsAnimals>()._animalLevel], _selectedAnimal.GetComponent<PlatformsAnimals>()._animalLevel);

                    Destroy(_selectedAnimal.gameObject);

                    var newAnimalPlatform = Instantiate(_animalPlatform, _selectedPlatform).GetComponent<PlatformsAnimals>();
                    var newPlatformsCreature = Instantiate(_sceeControllern._mergeCreaturesArray[creatureLevel], newAnimalPlatform.transform).GetComponent<MergeCreature>();
                    newAnimalPlatform.SetMergeCreatutureAfterSpawn(newPlatformsCreature, creatureLevel);
                    _selectedPlatform.GetComponent<Platform>().UpdatePlatformStatus(newAnimalPlatform.transform);

                    //_selectedPlatform.GetComponent<Platform>().SetAnimalToPlatform(_sceeControllern._mergeCreaturesArray[creatureLevel]);

                    _selectedPlatform = null;
                    _selectedAnimal = null;

                }
            }
            else if (!selectedPlatform._isItBoughtPlatform)
            {
                selectedPlatform.transform.DOKill();
                transform.localScale = new Vector3(1, 1, 1);
                selectedPlatform.transform.DOPunchScale(Vector3.up, 0.5f);

                _selectedPlatform.GetComponent<Platform>().SetAnimalToPlatform(_selectedAnimal);
                _selectedPlatform = null;
                _selectedAnimal = null;
            }

        }
        else
        {
            _selectedPlatform.GetComponent<Platform>().SetAnimalToPlatform(_selectedAnimal);
            _selectedPlatform = null;
            _selectedAnimal = null;
        }

        _platformsHandler.RewriteFreePlatformsList();
        _platformsHandler.CheckHuntingGroup();
    }


    private void HitRayAndTryToBuyPlatform()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, _platformLayer))
        {

            Platform selectedPlatform = hit.collider.gameObject.GetComponent<Platform>();
            if(_selectedPlatform.gameObject == selectedPlatform.gameObject)
            {
                selectedPlatform.TryToBuyPlatform(_sceeControllern._currentMeatCount);
            }
        }
    }
}
