using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class AnimalMovement : MonoBehaviour
{
    [Header("MOVEMENT")]
    public float _speed;
    public float _levelSpeed;
    public bool _isItForstCreature;

    [SerializeField] private GameObject _ghost;
    [SerializeField] private PathCreator _pathCreator;
    [SerializeField] private Predator _predator;
    [SerializeField] private Transform _lookPoint;
    [SerializeField] private GameObject _particleMarkert;
    [SerializeField] private DragAndShoot _dragAndShoot;
    [SerializeField] private Collider _collider;
    [SerializeField] private LadningPoint _landingPoint;

    private Ghost _currentGhost;
    private float _distanceTreveled;
    private bool _cameraPosition;

<<<<<<< HEAD
    [SerializeField] private bool _isItSpecialObject;

    private void Awake()
    {
        GameEvents.OnUpdateGhostStatus += UpdateGhost;
    }
    private void OnDestroy()
    {
        GameEvents.OnUpdateGhostStatus -= UpdateGhost;
    }
=======
>>>>>>> parent of fcc2af8f (Finish)
    public void SetAimForBeginers()
    {
        _landingPoint.TurnOnFeature();
    }
    void Update()
    {
        if(!_isItSpecialObject)
        {
            Move();
        }
    }
    private void Move()
    {
        _distanceTreveled += _speed * Time.deltaTime;
        transform.position = _pathCreator.path.GetPointAtDistance(_distanceTreveled);
        transform.rotation = _pathCreator.path.GetRotationAtDistance(_distanceTreveled);
    }
    public void SetPathForAnimal(PathCreator pathCreator, float pathoffset, bool cameraPosition)
    {
        _pathCreator = pathCreator;
        _distanceTreveled = pathoffset;
        _cameraPosition = cameraPosition;
    }
    public void SetPathForAnimal(PathCreator pathCreator, float pathoffset, bool cameraPosition, bool withGhost)
    {
        var newGhost = Instantiate(_ghost, null).GetComponent<Ghost>();

        _pathCreator = pathCreator;
        _distanceTreveled = pathoffset;
        _cameraPosition = cameraPosition;
        _ghost = newGhost.gameObject;

        newGhost._pathCreator = pathCreator;
        newGhost._distanceTreveled = pathoffset;

        _currentGhost = _ghost.GetComponent<Ghost>();
    }

    public void StartGhost(float speed)
    {
        _currentGhost._distanceTreveled += 0.095f;
        _currentGhost._visual.SetActive(true);
        _currentGhost._speed = speed;
        _currentGhost._animator.speed = speed * 10;
        _currentGhost._animator.SetTrigger("Run");
        
    }

    public void InitMovement()
    {
        if(_isItForstCreature)
        {
            GameEvents.CallOnStartHunt(_levelSpeed);
        }
    }

    public Transform ReturnCurrentTransformAndCameraPos(out bool cameraPosition)
    {
        cameraPosition = _cameraPosition;
        return transform;
    }
    public Transform ReturnLookPoint()
    {
        return _lookPoint;
    }
    public void SetInterectibleStatus(bool status)
    {
        _particleMarkert.SetActive(status);
        _dragAndShoot.enabled = status;
        _pathCreator.enabled = status;
        _dragAndShoot._canControl = true;

    }
}
