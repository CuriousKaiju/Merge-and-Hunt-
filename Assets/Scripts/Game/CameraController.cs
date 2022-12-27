using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _deleyTime;
    [SerializeField] private float _desiredTimeForMovement;
    [SerializeField] private Vector3 _cameraOffsetLeft;
    [SerializeField] private Vector3 _cameraOffsetRight;
    [SerializeField] private PredatorPathHandler _predatorPathHandler;
    [SerializeField] private GameObject _windParticles;

    private Quaternion _initRotation;
    private AnimalMovement _currentAnimal;
    private float _elapsedTimeForMovement;
    private bool _cameraOrientation;
    private Transform _desiredPoint;
    private Transform _lookPoint;
    private Vector3 _initPosition;
    private bool _cameraHaveToMove;

    [Header("SMOOTH ROTATION")]
    [SerializeField] private float _inertionTime;
    [SerializeField] private AnimationCurve _inertionCurve;
    private Vector2 _currentSpeed;
    private bool _needRotate;
    private float _timePassed;

    [Header("CONTROL VARIABLES")]
    [SerializeField] private float _rotationSpeed = 0.5f;
    [SerializeField] private float _direction = -1;
    private bool _isFingerOnTheScreen;
    private float _pitch = 0.0f;
    private float _yaw = 0.0f;

    private void Awake()
    {
        GameEvents.OnWindParticles += ChangeParticlesStatus;
    }
    private void OnDestroy()
    {
        GameEvents.OnWindParticles -= ChangeParticlesStatus;
    }

    private void ChangeParticlesStatus(bool status)
    {
        _windParticles.SetActive(status);
    }

    private void Update()
    {
        if (_cameraHaveToMove)
        {
            CameraMoveNavigation();
        }


        /*
        if (Input.GetMouseButtonDown(0))
        {
            _isFingerOnTheScreen = true;
        }


        if (Input.GetMouseButton(0))
        {
            if (_isFingerOnTheScreen)
            {
                RotationHandler();
            }
        }
        else
        {
            InertionRotation();
        }
        */
    }

    public void StartMoveToThePredator(AnimalMovement animalMovement)
    {
        _lookPoint = animalMovement.ReturnLookPoint();
        transform.parent = _lookPoint;
        _currentAnimal = animalMovement;
        //_currentAnimal.SetInterectibleStatus(true);
        _desiredPoint = animalMovement.ReturnCurrentTransformAndCameraPos(out _cameraOrientation);
        _initPosition = transform.localPosition;
        transform.DOLocalRotateQuaternion(Quaternion.Euler(0, 0, 0), 0.9f);
        _lookPoint.GetComponent<LookPoint>().StartFollowToPrey(_predatorPathHandler.ReturnNearestPreyLookPoint(animalMovement.transform));
        _cameraHaveToMove = true;

        //transform.DOLookAt(_lookPoint.position, _desiredTimeForMovement);
    }
    public void StartMoveToThePredator(AnimalMovement animalMovement, Transform prey)
    {
        transform.parent = prey;

        StartCoroutine(DeleyForNextPredador(animalMovement));

        //transform.DOLookAt(_lookPoint.position, _desiredTimeForMovement);
    }
    public void Fix(Transform prey)
    {
        transform.parent = prey;
    }
    private IEnumerator DeleyForNextPredador(AnimalMovement animalMovement)
    {
        
        yield return new WaitForSeconds(_deleyTime);

        _lookPoint = animalMovement.ReturnLookPoint();
        transform.parent = _lookPoint;
        _currentAnimal = animalMovement;
        //_currentAnimal.SetInterectibleStatus(true);
        _desiredPoint = animalMovement.ReturnCurrentTransformAndCameraPos(out _cameraOrientation);
        _initPosition = transform.localPosition;
        _cameraHaveToMove = true;
        transform.DOLocalRotateQuaternion(Quaternion.Euler(0, 0, 0), 0.9f);
        _lookPoint.GetComponent<LookPoint>().StartFollowToPrey(_predatorPathHandler.ReturnNearestPreyLookPoint(animalMovement.transform));
        //_lookPoint.GetComponent<LookPoint>().StartFollowToPrey(_predatorPathHandler.ReturnNearestPreyLookPoint(_lookPoint.transform));

    }

    public void CameraMoveNavigation()
    {
        _elapsedTimeForMovement += Time.deltaTime;
        float percentageComplete = _elapsedTimeForMovement / _desiredTimeForMovement;
        if (_cameraOrientation)
        {
            transform.localPosition = Vector3.Lerp(_initPosition, _cameraOffsetRight, percentageComplete);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(_initPosition, _cameraOffsetLeft, percentageComplete);
        }

        //transform.rotation = Quaternion.Slerp(_initRotation, Quaternion.Euler(0, 0, 0), percentageComplete);
        //transform.DOLookAt(_lookPoint.position, _desiredTimeForMovement);

        if (percentageComplete >= 1)
        {
            _elapsedTimeForMovement = 0;
            _cameraHaveToMove = false;
            transform.parent = _lookPoint;
            _yaw += _lookPoint.eulerAngles.y;
            _currentAnimal.SetInterectibleStatus(true);

        }
    }




    private void RotationHandler()
    {
        Vector2 deltaPosition = Input.GetTouch(0).deltaPosition;
        CameraRotate(deltaPosition);

        _needRotate = true;

        _currentSpeed = deltaPosition;

        _timePassed = 0;
    }
    private void InertionRotation()
    {
        if (!_needRotate)
        {
            return;
        }

        if (_timePassed >= _inertionTime)
        {
            _needRotate = false;
            return;
        }

        float normalizedTime = _timePassed / _inertionTime;
        Vector2 currentSpeed = _inertionCurve.Evaluate(normalizedTime) * _currentSpeed;

        CameraRotate(currentSpeed);

        _timePassed += Time.deltaTime;

    }

    private void CameraRotate(Vector2 rotationValue)
    {
        //_pitch -= rotationValue.y * _direction * _rotationSpeed * Time.deltaTime;

        _yaw += rotationValue.x * _direction * _rotationSpeed * Time.deltaTime;

        //_pitch = Mathf.Clamp(_pitch, -70, 70);

        _lookPoint.eulerAngles = new Vector3(0.0f,  _yaw, 0.0f);
    }


}
