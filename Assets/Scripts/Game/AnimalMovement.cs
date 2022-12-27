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


    [SerializeField] private PathCreator _pathCreator;
    [SerializeField] private Predator _predator;
    [SerializeField] private Transform _lookPoint;
    [SerializeField] private GameObject _particleMarkert;
    [SerializeField] private DragAndShoot _dragAndShoot;
    [SerializeField] private Collider _collider;

    
    private float _distanceTreveled;
    private bool _cameraPosition;

    void Update()
    {
        Move();
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
